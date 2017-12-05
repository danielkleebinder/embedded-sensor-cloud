using System.Net;

/// <summary>
/// Web server namespace.
/// </summary>
namespace MyWebServer
{
    /// <summary>
    /// Main program entry point.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main method as strating point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        static void Main(string[] args)
        {
            WebServer server = new WebServer();
            server.Address = IPAddress.Loopback;
            server.Port = 8080;
            server.Start();
        }
    }
}
