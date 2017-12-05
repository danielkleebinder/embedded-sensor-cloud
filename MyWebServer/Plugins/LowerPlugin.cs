using System;
using BIF.SWE1.Interfaces;

namespace MyWebServer.Plugins
{
    /// <summary>
    /// A plugin which lowers text.
    /// </summary>
    public class LowerPlugin : IPlugin
    {
        /// <summary>
        /// Checks if the given request can be handled by this plugin.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>Float value between 0.0 and 1.0.</returns>
        public Single CanHandle(IRequest req)
        {
            string body = req.ContentString;
            if (body != null && body.StartsWith("text="))
            {
                return 1.0f;
            }
            return 0.0f;
        }

        /// <summary>
        /// Handles the response by create a HTTP response with the text from the request
        /// in lower case.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>Reponse with lower case text as content.</returns>
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
