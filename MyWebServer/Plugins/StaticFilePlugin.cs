using System;
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
            return 0.5f;
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

            // Check if the request files exists on the OS and directly return it
            if (File.Exists(reqUrl))
            {
                return CreateResponse(response, reqUrl);
            }

            // Check if the given directory exists
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
            return CreateResponse(response, file);
        }

        private IResponse CreateResponse(IResponse response, string file)
        {
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
