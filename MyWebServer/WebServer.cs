using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer
{
    class WebServer
    {
        private TcpListener serverSocket;
        private bool running = false;

        /// <summary>
        /// Starts the web server.
        /// </summary>
        public void Start()
        {
            running = true;

            // Start TCP listener (server socket)
            serverSocket = new TcpListener(Address, Port);
            serverSocket.Start();

            // Start server main procedure
            while (running)
            {
                // Block until clients try to connect
                Console.WriteLine("Waiting for connections...");
                Socket clientSocket = serverSocket.AcceptSocket();

                // Start a new thread for the request handling
                Console.WriteLine("Socket connected: " + clientSocket.LocalEndPoint);
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
            Socket socket = (Socket)clientSocket;
            using (NetworkStream ns = new NetworkStream(socket))
            {
                IRequest req = new Request(ns);

                // Return "400 Bad Request" if the request is invalid
                if (!req.IsValid)
                {
                    SendBadRequest(ns);
                    return;
                }

                // Let the plugins handle all the server response stuff
                float highest = 0.0f;
                IPlugin current = null;
                foreach (IPlugin plugin in PluginManager.Plugins)
                {
                    float hc = plugin.CanHandle(req);
                    if (hc > highest)
                    {
                        highest = hc;
                        current = plugin;
                    }
                }


                // Check if any plugin is able to handle the given request
                if (current == null)
                {
                    SendBadRequest(ns);
                    return;
                }

                // Send the plugin response
                IResponse res = current.Handle(req);
                if (res != null)
                {
                    res.Send(ns);
                }
            }
            socket.Close();
        }

        private void SendBadRequest(Stream s)
        {
            Response err = new Response();
            err.StatusCode = 400;
            err.Send(s);
        }

        /// <summary>
        /// Sets and returns the local server address.
        /// </summary>
        public IPAddress Address { get; set; } = IPAddress.Loopback;

        /// <summary>
        /// Sets and returns the local server port.
        /// </summary>
        public int Port { get; set; } = 80;

        /// <summary>
        /// Sets and returns the plugin manager for this webserver instance.
        /// </summary>
        public IPluginManager PluginManager { get; set; } = new PluginManager();
    }
}
