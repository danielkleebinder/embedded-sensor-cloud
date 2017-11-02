using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using MyWebServer;
using System.Reflection;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer server = new WebServer();
            server.Address = IPAddress.Loopback;
            server.Port = 8080;
            server.Start();
        }
    }
}
