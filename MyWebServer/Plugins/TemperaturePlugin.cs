using System;
using BIF.SWE1.Interfaces;
using MyWebServer.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;

namespace MyWebServer.Plugins
{
    /// <summary>
    /// A plugin which handles sensor data using the database.
    /// </summary>
    class TemperaturePlugin : IPlugin
    {
        private DatabaseAccess access;

        /// <summary>
        /// Creates a new temperature plugin.
        /// </summary>
        public TemperaturePlugin()
        {
            // access = new SQLServerDatabaseAccess();
            access = new VirtualDatabaseAccess();
            access.Initialize();

            // Initialize 10.000 DataSets
            // LoadTestDataSets();

            ThreadPool.QueueUserWorkItem(ReadSensor);
        }

        /// <summary>
        /// Reads from the a "virtual" sensor every 10 seconds and copies the
        /// data into the database.
        /// </summary>
        /// <param name="obj">Threading object parameter.</param>
        private void ReadSensor(object obj)
        {
            Random rnd = new Random();
            while (true)
            {
                // Add a new temperature entry every 10 seconds
                if (access != null)
                {
                    access.SaveTemperature(new Temperature(DateTime.Now, (rnd.NextDouble() - 0.5) * 60.0));
                }
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle
        /// the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>A score between 0 and 1.</returns>
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url == null || req.Url.Segments.Length < 1)
            {
                return 0.0f;
            }
            if (req.Url.Segments[0] == "temp")
            {
                return 1.0f;
            }
            return 0.0f;
        }

        /// <summary>
        /// Called by the server when the plugin should handle the request.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>A new response object.</returns>
        public IResponse Handle(IRequest req)
        {
            bool restXML = false;
            if (req.Url.Parameter.ContainsKey("type"))
            {
                string type = req.Url.Parameter["type"].ToLower();
                restXML = type == "rest";
            }

            // Create response object
            IResponse result = new Response();
            result.StatusCode = 400;

            // Check if URL contains all needed information
            if (!(req.Url.Parameter.ContainsKey("from")
                && req.Url.Parameter.ContainsKey("until")))
            {
                return result;
            }

            // Parse date time values from GET parameters
            DateTime from, until;
            try
            {
                from = DateTime.Parse(req.Url.Parameter["from"]);
                until = DateTime.Parse(req.Url.Parameter["until"]);
            }
            catch (FormatException)
            {
                return result;
            }

            // Check if parsing was successful
            if (from == null || until == null)
            {
                return result;
            }

            // Load temperature data from database and build response
            List<Temperature> data = access.LoadAllTemperaturesRange(from, until);
            if (restXML)
            {
                return CreateRestXML(result, req, data);
            }
            return CreateHTML(result, req, data);
        }

        /// <summary>
        /// Creates a new rest response using the XML API.
        /// </summary>
        /// <param name="result">Response.</param>
        /// <param name="req">Request.</param>
        /// <param name="data">Data set of temperatures.</param>
        /// <returns>Finished response.</returns>
        private IResponse CreateRestXML(IResponse result, IRequest req, List<Temperature> data)
        {
            // 200 Success Code
            result.StatusCode = 200;
            result.ContentType = HTTP.CONTENT_TYPE_TEXT_XML;

            // Build XML Response
            XmlDocument document = new XmlDocument();
            XmlElement root = (XmlElement) document.AppendChild(document.CreateElement("Temperatures"));
            foreach (Temperature current in data)
            {
                // Entry root element
                XmlElement entry = (XmlElement) root.AppendChild(document.CreateElement("Entry"));
                entry.SetAttribute("id", current.ID.ToString());

                // Date element containing the date in yyyy-MM-dd hh:mm:ss format
                XmlElement date = (XmlElement) entry.AppendChild(document.CreateElement("Date"));
                date.AppendChild(document.CreateTextNode(current.Date.ToString("yyyy-MM-dd hh:mm:ss")));

                // Temperature element in Kelvin
                XmlElement kelvin = (XmlElement) entry.AppendChild(document.CreateElement("Kelvin"));
                kelvin.AppendChild(document.CreateTextNode(current.Kelvin.ToString().Replace(',', '.')));

                // Temperature element in Celsius
                XmlElement celsius = (XmlElement) entry.AppendChild(document.CreateElement("Celsius"));
                celsius.AppendChild(document.CreateTextNode(current.Celsius.ToString().Replace(',', '.')));

                // Temperature element in Fahrenheit
                XmlElement fahrenheit = (XmlElement) entry.AppendChild(document.CreateElement("Fahrenheit"));
                fahrenheit.AppendChild(document.CreateTextNode(current.Fahrenheit.ToString().Replace(',', '.')));
            }

            // Finish Response
            result.SetContent(document.OuterXml);
            return result;
        }

        /// <summary>
        /// Creates and builds the HTML response.
        /// </summary>
        /// <param name="result">Response.</param>
        /// <param name="req">Request.</param>
        /// <param name="data">Data set of temperatures.</param>
        /// <returns>Finished response.</returns>
        private IResponse CreateHTML(IResponse result, IRequest req, List<Temperature> data)
        {
            // 200 Success Code
            result.StatusCode = 200;
            result.ContentType = HTTP.CONTENT_TYPE_TEXT_HTML;

            // Build Response
            StringBuilder content = new StringBuilder();
            content.Append("<html><body>");
            content.Append("<h1>Temperature</h1>");
            content.Append("<table>");
            content.Append("<tr><th>ID</th><th>Date</th><th>Kelvin</th><th>Celsius</th><th>Fahrenheit</th></tr>");
            foreach (Temperature current in data)
            {
                content.Append("<tr>");
                content.Append("<td>").Append(current.ID).Append("</td>");
                content.Append("<td>").Append(current.Date).Append("</td>");
                content.Append("<td>").Append(current.Kelvin).Append("</td>");
                content.Append("<td>").Append(current.Celsius).Append("</td>");
                content.Append("<td>").Append(current.Fahrenheit).Append("</td>");
                content.Append("</tr>");
            }
            content.Append("</table>");
            content.Append("</body></html>");

            // Finish Response
            result.SetContent(content.ToString());
            return result;
        }

        /// <summary>
        /// Writes around 10.000 test data sets to the database.
        /// </summary>
        private void LoadTestDataSets()
        {
            Random rnd = new Random();
            DateTime tenYearsAgo = DateTime.Now.AddYears(-10);
            for (int i = 0; i < 365 * 10 * 3; i++)
            {
                // Create random temperature
                Temperature tmp = new Temperature();
                tmp.Date = tenYearsAgo
                    .AddDays(i / 3)
                    .AddHours((i % 3) * 6 + 1);
                tmp.Celsius = Math.Sin(Math.PI * (i / 100.0)) * 30.0;
                tmp.Celsius += (rnd.NextDouble() - 0.5) * 10.0;
                access.SaveTemperature(tmp);
            }
        }
    }
}