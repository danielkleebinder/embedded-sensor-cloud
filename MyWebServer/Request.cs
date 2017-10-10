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
        private Stream network;
        private bool valid;

        public Request() : this(null) { }

        public Request(Stream network)
        {
            this.network = network;

            // Open a stream reader to the given network
            StreamReader reader = new StreamReader(network);

            if (Headers == null) {
                Headers = new Dictionary<string, string>();
            }

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
                if (line.Contains(':')) {
                    string[] headerData = line.Split(':');
                    string key = headerData[0].Trim().ToLower();
                    string value = headerData[1].Trim();
                    Headers.Add(key, value);
                } else {
                    ParseHTTPHeaderLine(line);
                }

                lineCount++;
            }

            // Do validity check
            valid = true;
            if (lineCount <= 0) {
                valid = false;
            }
            if (!Settings.HTTP_METHODS.Contains(Method)) {
                valid = false;
            }
            if (Url == null) {
                valid = false;
            }
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
                Method = parameters[0].ToUpper();
            }
            if (parameters.Length >= 2) {
                Url = new Url(parameters[1]);
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
                if (!Headers.ContainsKey(Settings.CONTENT_LENGTH)) {
                    throw new InvalidOperationException("Content length not available in header");
                }
                return Int32.Parse(Headers[Settings.CONTENT_LENGTH]);
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
                if (!Headers.ContainsKey(Settings.CONTENT_TYPE)) {
                    throw new InvalidOperationException("Content type not available in header");
                }
                return Headers[Settings.CONTENT_TYPE];
            }
        }

        public Int32 HeaderCount
        {
            get { return Headers.Count; }
        }

        public IDictionary<String, String> Headers
        {
            get; private set;
        }

        public Boolean IsValid
        {
            get { return valid; }
        }

        public String Method
        {
            get; private set;
        }

        public IUrl Url
        {
            get; private set;
        }

        public String UserAgent
        {
            get
            {
                if (!Headers.ContainsKey(Settings.USER_AGENT)) {
                    throw new InvalidOperationException("User agent not available in header");
                }
                return Headers[Settings.USER_AGENT];
            }
        }
    }
}
