using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Console.WriteLine(type);
            }
            Console.WriteLine(restXML);


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
            result.StatusCode = 200;
            result.ContentType = HTTP.CONTENT_TYPE_TEXT_HTML;

            // TODO: Use Database Connection Here
            result.SetContent("<html><h1>Temperature</h1></html>");
            return result;
        }
    }
}
