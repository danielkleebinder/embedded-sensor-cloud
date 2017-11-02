using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer.Plugins
{
    class StaticFilePlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url == null || req.Url.RawUrl == null)
            {
                return 0.0f;
            }
            if (req.Url.RawUrl.Contains("static-files"))
            {
                return 1.0f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response();
            response.ContentType = HTTP.ContentTypeEncoding(HTTP.CONTENT_TYPE_TEXT_PLAIN, "UTF-8");
            response.AddHeader(HTTP.CONNECTION, HTTP.CONNECTION_CLOSED);
            response.AddHeader(HTTP.CONTENT_LANGUAGE, HTTP.CONTENT_LANGUAGE_EN);

            // Return simple 404 Not Found code if the file does not exist
            if (!File.Exists(req.Url.RawUrl))
            {
                response.StatusCode = 404;
                return response;
            }

            // Create proper response
            response.StatusCode = 200;
            response.SetContent(File.ReadAllBytes(req.Url.RawUrl));
            return response;
        }
    }
}
