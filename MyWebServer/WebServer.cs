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
                Socket clientSocket = serverSocket.AcceptSocket();
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
            }
        }

        /// <summary>
        /// Sets and returns the local server address.
        /// </summary>
        public IPAddress Address { get; set; } = IPAddress.Parse("localhost");

        /// <summary>
        /// Sets and returns the local server port.
        /// </summary>
        public int Port { get; set; } = 8080;
    }
}
