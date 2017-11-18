using System;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer.Plugins
{
    public class IndexPlugin : IPlugin
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

            return 0.5f;
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

            // Send 404 Not Found response
            string file = null;
            string reqUrl = req.Url.RawUrl.Replace("/", @"\");
            string dir = AppContext.Current.WorkingDirectory + reqUrl;
            if (Directory.Exists(dir))
            {
                if (!dir.EndsWith(@"\"))
                {
                    dir += @"\";
                }
            }

            // Check for all available index files
            foreach (string doc in HTTP.DEFAULT_DOCUMENTS)
            {
                string osFile = dir + doc.Replace("/", @"\");
                if (File.Exists(osFile))
                {
                    file = osFile;
                    break;
                }
            }

            // Return 404 NOT FOUND if the file does not exist
            if (file == null)
            {
                response.StatusCode = 404;
                return response;
            }

            // Get file extension
            string ext = Path.GetExtension(file);

            // Send response file
            response.StatusCode = 200;
            response.ContentType = HTTP.ContentTypeEncoding(HTTP.MimeTypeFromExtension(ext), "UTF-8");
            response.SetContent(File.ReadAllBytes(file));
            return response;
        }
    }
}