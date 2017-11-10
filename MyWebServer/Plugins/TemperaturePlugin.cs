using System;
using BIF.SWE1.Interfaces;

namespace MyWebServer.Plugins
{
    class TemperaturePlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url == null || req.Url.Segments.Length < 1) {
                return 0.0f;
            }
            if (req.Url.Segments[0] == "temp") {
                return 1.0f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            bool restXML = false;
            if (req.Url.Parameter.ContainsKey("type")) {
                string type = req.Url.Parameter["type"].ToLower();
                restXML = type == "rest";
            }


            if (restXML) {
                return CreateRestXML(req);
            }
            return CreateHTML(req);
        }

        private IResponse CreateRestXML(IRequest req)
        {
            IResponse result = new Response();
            result.StatusCode = 200;
            result.ContentType = HTTP.CONTENT_TYPE_TEXT_XML;

            // TODO: Use Database Connection Here
            result.SetContent("<REST><temp></temp></REST>");
            return result;
        }

        private IResponse CreateHTML(IRequest req)
        {
            IResponse result = new Response();
            result.StatusCode = 400;

            // Check if URL contains all needed information
            if (!( req.Url.Parameter.ContainsKey("from")
                && req.Url.Parameter.ContainsKey("until") )) {
                return result;
            }

            // Parse date time values from GET parameters
            DateTime from, until;
            try {
                from = DateTime.Parse(req.Url.Parameter["from"]);
                until = DateTime.Parse(req.Url.Parameter["until"]);
            } catch (FormatException) {
                return result;
            }

            // Check if parsing was successful
            if (from == null || until == null) {
                return result;
            }

            // TODO: Use Database Connection Here
            result.StatusCode = 200;
            result.ContentType = HTTP.CONTENT_TYPE_TEXT_HTML;
            result.SetContent("<html><h1>Temperature</h1><h3>" + from + "</h3><h3>" + until + "</h3></html>");
            return result;
        }
    }
}
