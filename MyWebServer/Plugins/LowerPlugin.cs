using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer.Plugins
{
    class LowerPlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            string body = req.ContentString;
            if (body != null && body.StartsWith("text="))
            {
                return 1.0f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            string body = req.ContentString;
            body = body.Remove(0, body.IndexOf('=') + 1);

            // Create response
            Response response = new Response();
            response.StatusCode = 200;
            response.ContentType = HTTP.ContentTypeEncoding(HTTP.CONTENT_TYPE_TEXT_PLAIN, "UTF-8");
            response.AddHeader(HTTP.CONNECTION, HTTP.CONNECTION_CLOSED);
            response.AddHeader(HTTP.CONTENT_LANGUAGE, HTTP.CONTENT_LANGUAGE_EN);

            // Send empty handling protocol
            if (string.IsNullOrEmpty(body.Trim()))
            {
                response.SetContent("Bitte geben Sie einen Text ein");
                return response;
            }

            // Send correctly executed protocol
            response.SetContent(body.ToLower());
            return response;
        }
    }
}
