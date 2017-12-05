using System;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer.Plugins
{
    /// <summary>
    /// The static file plugin is used to send web page files like CSS, JS and images
    /// to the clients. This plugin is mainly responsible for creating HTTP responses
    /// and to build web pages.
    /// </summary>
    public class StaticFilePlugin : IPlugin
    {
        /// <summary>
        /// Checks if the given request can be handled by this plugin. If the URL is valid, no
        /// problems should occur.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>Float value between 0.0 and 1.0.</returns>
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url == null || req.Url.RawUrl == null)
            {
                return 0.0f;
            }
            return 0.5f;
        }

        /// <summary>
        /// Handles the request by sending requested files to the client.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>Response.</returns>
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

        /// <summary>
        /// Initializes the given response for the given file.
        /// </summary>
        /// <param name="response">Response.</param>
        /// <param name="file">File to be sent.</param>
        /// <returns>Setup response with file data and status codes.</returns>
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
