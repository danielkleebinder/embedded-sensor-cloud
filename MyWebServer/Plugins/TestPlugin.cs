using System;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer.Plugins
{
    public class TestPlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (CheckForParameter(req))
            {
                return 1.0f;
            }

            if (CheckForURLPath(req))
            {
                return 0.8f;
            }

            return 0.0f;
        }

        private bool CheckForParameter(IRequest req)
        {
            // Check if test plugin parameter is available
            if (!req.Url.Parameter.ContainsKey("test_plugin"))
            {
                return false;
            }

            // Read test plugin parameter
            string param = req.Url.Parameter["test_plugin"];
            if (string.IsNullOrEmpty(param))
            {
                return false;
            }

            // Parse test plugin parameter
            bool result = false;
            try
            {
                result = bool.Parse(param);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex);
            }

            // Return the result
            return result;
        }

        private bool CheckForURLPath(IRequest req)
        {
            return req.Url.Segments[0] == "test" || req.Url.RawUrl == "/";
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response();
            response.AddHeader(HTTP.CONNECTION, HTTP.CONNECTION_CLOSED);
            response.AddHeader(HTTP.CONTENT_LANGUAGE, HTTP.CONTENT_LANGUAGE_EN);
            response.AddHeader(HTTP.CONTENT_ENCODING, HTTP.CONTENT_ENCODING_UTF8);
            response.StatusCode = 200;
            response.ContentType = HTTP.ContentTypeEncoding(HTTP.CONTENT_TYPE_TEXT_HTML, "UTF-8");
            response.SetContent("<!DOCTYPE html><html><body><h1>Test Plugin</h1><h3>#+ßöäüabc123</h3></body></html>");
            return response;
        }
    }
}