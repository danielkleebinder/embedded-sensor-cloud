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
        public Request(Stream network)
        {
            // Open a stream reader to the given network
            StreamReader reader = new StreamReader(network, Encoding.UTF8);
            if (Headers == null)
            {
                Headers = new Dictionary<string, string>();
            }

            int lineCount = 0;
            string line;

            // Read Header
            while ((line = reader.ReadLine()) != null)
            {
                // Check if line is empty which means "end of header"
                Console.WriteLine(line);
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                // Parse HTTP Protocol Parameters
                if (line.Contains(':') && lineCount > 0)
                {
                    string[] headerData = line.Split(':');
                    string key = headerData[0].Trim().ToLower();
                    string value = headerData[1].Trim();
                    Headers.Add(key, value);
                }
                else
                {
                    ParseHTTPHeaderLine(line);
                }

                lineCount++;
            }

            // Read Body Data
            if (ContentLength > 0)
            {
                ContentString = reader.ReadToEnd();
                ContentBytes = Encoding.UTF8.GetBytes(ContentString);
                ContentStream = new MemoryStream(ContentBytes);
            }

            // Do validity check
            IsValid = true;
            if (lineCount <= 0)
            {
                IsValid = false;
            }
            if (!Settings.HTTP_METHODS.Contains(Method))
            {
                IsValid = false;
            }
            if (Url == null)
            {
                IsValid = false;
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

            if (parameters.Length >= 1)
            {
                Method = parameters[0].ToUpper();
            }
            if (parameters.Length >= 2)
            {
                // URL is allowed to include whitespaces
                string url = line.Remove(0, line.IndexOf(' '));
                url = url.Remove(url.LastIndexOf(' ')).Trim();
                Url = new Url(url);
            }
        }

        public Byte[] ContentBytes
        {
            get; private set;
        }

        public Int32 ContentLength
        {
            get
            {
                if (!Headers.ContainsKey(HTTP.CONTENT_LENGTH_LC))
                {
                    return 0;
                }
                return Int32.Parse(Headers[HTTP.CONTENT_LENGTH_LC]);
            }
        }

        public Stream ContentStream
        {
            get; private set;
        }

        public String ContentString
        {
            get; private set;
        }

        public String ContentType
        {
            get
            {
                if (!Headers.ContainsKey(HTTP.CONTENT_TYPE_LC))
                {
                    throw new InvalidOperationException("Content type not available in header");
                }
                return Headers[HTTP.CONTENT_TYPE_LC];
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
            get; private set;
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
                if (!Headers.ContainsKey(HTTP.USER_AGENT_LC))
                {
                    throw new InvalidOperationException("User agent not available in header");
                }
                return Headers[HTTP.USER_AGENT_LC];
            }
        }
    }
}
