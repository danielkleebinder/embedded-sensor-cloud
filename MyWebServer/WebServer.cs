using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using BIF.SWE1.Interfaces;


namespace MyWebServer
{
    class WebServer
    {
        private TcpListener serverSocket;
        private bool running = false;

        /// <summary>
        /// Creates a new instance of the web server class.
        /// </summary>
        public WebServer()
        {
        }

        /// <summary>
        /// Starts the web server.
        /// </summary>
        public void Start()
        {
            running = true;

            serverSocket = new TcpListener(Address, Port);
            serverSocket.Start();

            while (running) {
                Console.WriteLine("Waiting for connections...");
                Socket clientSocket = serverSocket.AcceptSocket();
                Console.WriteLine("Socket connected: " + clientSocket);
                ThreadPool.QueueUserWorkItem(HandleHTTPRequest, clientSocket);
            }
        }

        /// <summary>
        /// Stops the web server.
        /// </summary>
        public void Stop()
        {
            running = false;
            serverSocket.Stop();
        }

        /// <summary>
        /// Handles the HTTP request.
        /// </summary>
        public void HandleHTTPRequest(object clientSocket)
        {
            // Get request from client
            Socket socket = (Socket) clientSocket;
            using (NetworkStream ns = new NetworkStream(socket)) {
                IRequest req = new Request(ns);
                if (!req.IsValid) {
                    return;
                }
                foreach (IPlugin plugin in PluginManager.Plugins) {
                    if (plugin.CanHandle(req) > 0) {
                        plugin.Handle(req).Send(ns);
                    }
                }
            }
        }

        /// <summary>
        /// Sets and returns the local server address.
        /// </summary>
        public IPAddress Address { get; set; } = IPAddress.Loopback;

        /// <summary>
        /// Sets and returns the local server port.
        /// </summary>
        public int Port { get; set; } = 8080;

        /// <summary>
        /// Sets and returns the plugin manager for this webserver instance.
        /// </summary>
        public IPluginManager PluginManager { get; set; } = new PluginManager();
    }
}
