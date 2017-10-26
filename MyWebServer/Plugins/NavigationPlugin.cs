using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer.Plugins
{
    class NavigationPlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {

            throw new NotImplementedException();
        }
    }
}