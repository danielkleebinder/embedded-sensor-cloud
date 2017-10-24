using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace TestPlugin
{
    public class TestPlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (CheckForParameter(req)) {
                return 1.0f;
            }

            if (CheckForURLPath(req)) {
                return 0.8f;
            }

            return 0.0f;
        }

        private bool CheckForParameter(IRequest req)
        {
            // Check if test plugin parameter is available
            if (!req.Url.Parameter.ContainsKey("test_plugin")) {
                return false;
            }

            // Read test plugin parameter
            string param = req.Url.Parameter["test_plugin"];
            if (string.IsNullOrEmpty(param)) {
                return false;
            }

            // Parse test plugin parameter
            bool result = false;
            try {
                result = bool.Parse(param);
            } catch (FormatException ex) {
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
            response.StatusCode = 200;
            response.SetContent("<html><head></head><body><h1>Hello World</h1></body></html>");
            return response;
        }
    }
}
