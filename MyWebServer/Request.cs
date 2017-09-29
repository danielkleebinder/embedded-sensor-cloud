using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class Request : IRequest
    {

        private IDictionary<string, string> headers = new Dictionary<string, string>();
        private Stream network;

        private string userAgent;
        private string method;
        private IUrl url;
        private bool valid;

        public Request() : this(null) { }

        public Request(Stream network)
        {
            this.network = network;

            // Open a stream reader to the given network
            StreamReader reader = new StreamReader(network);

            int lineCount = 0;
            string line;
            while (!reader.EndOfStream) {
                line = reader.ReadLine();
                if (line == null) {
                    continue;
                }

                // Check if line is empty
                line = line.Trim();
                if (line == string.Empty) {
                    continue;
                }

                // Parse HTTP Protocol Parameters
                if (lineCount == 0 && line.Contains(Settings.HTTP)) {
                    ParseHTTPHeaderLine(line);
                } else if (Settings.USER_AGENT.Equals(line, StringComparison.InvariantCultureIgnoreCase)) {
                    ParseUserAgentParameter(line);
                }

                lineCount++;
            }

            // Do validity check
            valid = true;
            if (lineCount <= 0) {
                valid = false;
            }
            if (!Settings.HTTP_METHODS.Contains(method)) {
                valid = false;
            }
        }

        /// <summary>
        /// Parses the given line for the user agent parameter ("User-Agent: ").
        /// </summary>
        /// <param name="line">Request line</param>
        private void ParseUserAgentParameter(string line)
        {
            userAgent = line.Replace(Settings.USER_AGENT, "").Trim();
        }


        /// <summary>
        /// Parses the given line for the HTTP header line containing the
        /// HTTP method and the URL.
        /// </summary>
        /// <param name="line">Request line</param>
        private void ParseHTTPHeaderLine(string line)
        {
            string[] parameters = line.Split(' ');

            if (parameters.Length >= 1) {
                method = parameters[0].ToUpper();
            }
            if (parameters.Length >= 2) {
                url = new Url(parameters[1]);
            }
        }

        public Byte[] ContentBytes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Int32 ContentLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Stream ContentStream
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public String ContentString
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public String ContentType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Int32 HeaderCount
        {
            get
            {
                return headers.Count;
            }
        }

        public IDictionary<String, String> Headers
        {
            get
            {
                return headers;
            }
        }

        public Boolean IsValid
        {
            get
            {
                return valid;
            }
        }

        public String Method
        {
            get
            {
                return method;
            }
        }

        public IUrl Url
        {
            get
            {
                return url;
            }
        }

        public String UserAgent
        {
            get
            {
                return userAgent;
            }
        }
    }
}
