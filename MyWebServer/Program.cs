using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace MyWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer server = new WebServer();
            server.Address = IPAddress.Parse("localhost");
            server.Port = 8080;
            server.Start();
        }
    }
}
