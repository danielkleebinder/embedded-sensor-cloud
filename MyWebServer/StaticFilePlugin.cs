using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class StaticFilePlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            return 1.0f;
        }

        public IResponse Handle(IRequest req)
        {
            // System.Environment.CurrentDirectory
            Response response = new Response();
            response.StatusCode = 200;
            response.SetContent("<html><head></head><body><h1>Static File Plugin</h1></body></html>");
            return response;
        }
    }
}
