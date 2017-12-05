using System;
using BIF.SWE1.Interfaces;

/// <summary>
/// Contains all plugin related classes.
/// </summary>
namespace MyWebServer.Plugins
{
    /// <summary>
    /// A simple plugin for testing purposes.
    /// </summary>
    public class TestPlugin : IPlugin
    {
        /// <summary>
        /// Checks if the plugin can handle the given request. If the URL contains a parameter called "test_plugin" or
        /// if the URL is the root directory, this plugin will be able to handle incoming requests.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>Float value between 0.0 and 1.0.</returns>
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

        /// <summary>
        /// Checks if the parameters from the request are fitting.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>True if parameters are valid, otherwise false.</returns>
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

        /// <summary>
        /// Checks the URL path.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>True if the URL path is handlable, otherwise false.</returns>
        private bool CheckForURLPath(IRequest req)
        {
            return req.Url.Segments[0] == "test" || req.Url.RawUrl == "/";
        }

        /// <summary>
        /// Handles the response and creates a response object which delivers the standard test plugin
        /// page.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <returns>Created response.</returns>
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