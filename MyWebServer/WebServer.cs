using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

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
            Address = IPAddress.Parse("localhost");
        }

        /// <summary>
        /// Starts the web server.
        /// </summary>
        public void Start()
        {
            running = true;
            serverSocket = new TcpListener(Address, 8080);
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
            Socket socket = (Socket) clientSocket;
        }

        /// <summary>
        /// Sets and returns the local server address.
        /// </summary>
        public IPAddress Address { get; set; }
    }
}
