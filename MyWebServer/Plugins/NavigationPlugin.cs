using System;
using BIF.SWE1.Interfaces;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace MyWebServer.Plugins
{
    /// <summary>
    /// A navigation plugin for specific areas and streets.
    /// </summary>
    class NavigationPlugin : IPlugin
    {
        private static object refreshLock = new object();
        private static bool refreshing = false;
        private static Dictionary<string, List<string>> streetCityMap;

        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url == null || req.Url.Segments.Length < 1)
            {
                return 0.0f;
            }
            if (req.Url.Segments[0] == "navi")
            {
                if (HTTP.METHOD_GET.Equals(req.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    return 0.8f;
                }
                if (HTTP.METHOD_POST.Equals(req.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    return 1.0f;
                }
                return 0.5f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            IResponse result = new Response();
            result.StatusCode = 200;

            // Parse the street parameter
            string street = null;
            if (req.Url.Parameter.ContainsKey("street"))
            {
                street = req.Url.Parameter["street"];
            }
            else if (req.ContentString != null && req.ContentString.Contains("street="))
            {
                string[] splits = req.ContentString.Split('=');
                if (splits.Length > 1)
                {
                    street = splits[1];
                }
            }

            if (req.Url.Parameter.ContainsKey("refresh"))
            {
                bool refresh = Boolean.Parse(req.Url.Parameter["refresh"]);
                if (refresh)
                {
                    if (!LoadCityStreetCorrelation())
                    {
                        result.SetContent("Warnung: Karte wird gerade aufbereitet");
                        return result;
                    }
                }
            }

            lock (refreshLock)
            {
                if (refreshing)
                {
                    result.SetContent("Warnung: Karte wird gerade aufbereitet");
                    return result;
                }
            }

            if (streetCityMap == null)
            {
                if (!LoadCityStreetCorrelation())
                {
                    result.SetContent("Warnung: Karte wird gerade aufbereitet");
                    return result;
                }
            }


            // Build Response
            if (string.IsNullOrEmpty(street))
            {
                result.SetContent("Bitte geben Sie eine Anfrage ein");
                return result;
            }
            else
            {
                // Read XML using SAX and build the result
                StringBuilder content = new StringBuilder();

                bool containsKey = false;
                lock (streetCityMap)
                {
                    containsKey = streetCityMap.ContainsKey(street);
                }

                if (containsKey)
                {
                    content.Append("Orte gefunden,");
                    lock (streetCityMap)
                    {
                        foreach (string city in streetCityMap[street])
                        {
                            content
                                .Append(city)
                                .Append(',');
                        }
                    }
                    content.Length--;
                }
                else
                {
                    content.Append("Keine Orte gefunden");
                }

                // Finish Response
                result.SetContent(content.ToString());
            }
            return result;
        }

        /// <summary>
        /// Loads all city-street correlations.
        /// </summary>
        /// <returns>True if loading was successful, false if another thread is already loading the data set.</returns>
        private bool LoadCityStreetCorrelation()
        {
            // Set refreshing lock
            lock (refreshLock)
            {
                if (refreshing)
                {
                    return false;
                }
                refreshing = true;
            }

            // Create the new dictionary
            streetCityMap = new Dictionary<string, List<string>>(6533);

            // Refresh map
            string street = null;
            if (File.Exists("./streetmap/map.osm.xml"))
            {
                using (XmlReader reader = XmlReader.Create("./streetmap/map.osm.xml"))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        // Check if the node is an element
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        // Check if current element is a "tag" element
                        if (reader.Name == null || reader.Name != "tag")
                        {
                            continue;
                        }

                        // Retrieve key value
                        string key = reader.GetAttribute("k");

                        // Check if element contains street name
                        if (key != null && (key == "name" || key == "addr:street"))
                        {
                            street = reader.GetAttribute("v");
                        }

                        // Save city value
                        if (street != null && key != null && (key == "city" || key == "addr:city"))
                        {
                            // Lock static map so only one thread can load it at once
                            lock (streetCityMap)
                            {
                                if (!streetCityMap.ContainsKey(street))
                                {
                                    streetCityMap.Add(street, new List<string>());
                                }
                                streetCityMap[street].Add(reader.GetAttribute("v"));
                            }
                            street = null;
                        }
                    }
                }
            }

            // Return refreshing result
            lock (refreshLock)
            {
                refreshing = false;
            }
            return true;
        }
    }
}