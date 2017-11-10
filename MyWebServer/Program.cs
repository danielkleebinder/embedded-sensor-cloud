using System.Net;

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
