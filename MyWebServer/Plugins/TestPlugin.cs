using System;
using BIF.SWE1.Interfaces;
using MyWebServer;
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
            // return true;
        }

        public IResponse Handle(IRequest req)
        {
            /*Response response = new Response();
            response.AddHeader(HTTP.CONNECTION, HTTP.CONNECTION_CLOSED);
            response.AddHeader(HTTP.CONTENT_LANGUAGE, HTTP.CONTENT_LANGUAGE_EN);
            response.AddHeader(HTTP.CONTENT_ENCODING, HTTP.CONTENT_ENCODING_UTF8);

            // Check for file existence
            string reqUrl = req.Url.RawUrl;
            if (req.Url.RawUrl == "/")
            {
                reqUrl = "\\index.html";
            }

            // Send 404 Not Found response
            string file = AppContext.Current.WorkingDirectory + reqUrl.Replace("/", @"\");

            Console.WriteLine("REQUEST: " + file);
            if (!File.Exists(file))
            {
                response.StatusCode = 404;
                return response;
            }

            // Send response file
            response.StatusCode = 200;
            switch (req.Url.Extension)
            {
                case ".css":
                    response.ContentType = HTTP.ContentTypeEncoding(HTTP.CONTENT_TYPE_TEXT_CSS, "UTF-8");
                    break;
                case ".js":
                    response.ContentType = HTTP.ContentTypeEncoding(HTTP.CONTENT_TYPE_TEXT_CSS, "UTF-8");
                    break;
                default:
                    response.ContentType = HTTP.ContentTypeEncoding(HTTP.CONTENT_TYPE_TEXT_HTML, "UTF-8");
                    break;
            }
            response.SetContent(File.ReadAllBytes(file));
            return response;*/

            Response response = new Response();
            response.StatusCode = 200;
            response.ContentType = HTTP.CONTENT_TYPE_TEXT_HTML + "; charset=utf-8";
            response.AddHeader(HTTP.CONNECTION, HTTP.CONNECTION_CLOSED);
            response.AddHeader(HTTP.CONTENT_LANGUAGE, "en");
            response.AddHeader("Date", "Mon, 23 May 2017 22:38:34 GMT");
            response.AddHeader("Expires", "-1");
            response.AddHeader("Cache-Control", "private, max-age=0");
            response.AddHeader("Content-Encoding", "utf-8");
            response.SetContent("<!DOCTYPE html>\r\n\r\n<html><head></head><body><h1>Hello World</h1><p>ßÖÄü123asd</p></body></html>");
            return response;
        }
    }
}