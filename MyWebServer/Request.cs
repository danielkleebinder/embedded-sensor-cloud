using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    /// <summary>
    /// The request class is used to parse HTTP requests.
    /// </summary>
    public class Request : IRequest
    {
        /// <summary>
        /// Creates a new instance of "Request" using the specified network stream to
        /// retreive request data from the clients.
        /// </summary>
        /// <param name="network">Network stream.</param>
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
                char[] result = new char[ContentLength];
                reader.Read(result, 0, ContentLength);
                ContentString = new string(result);
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

        /// <summary>
        /// Contains the request data body as byte array.
        /// </summary>
        public Byte[] ContentBytes
        {
            get; private set;
        }

        /// <summary>
        /// Contains the data body length in bytes.
        /// </summary>
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

        /// <summary>
        /// Contains the data body as stream.
        /// </summary>
        public Stream ContentStream
        {
            get; private set;
        }

        /// <summary>
        /// Contains the data body as simple string. This representation of the data can cause problems
        /// when receiving images for example.
        /// </summary>
        public String ContentString
        {
            get; private set;
        }

        /// <summary>
        /// Contains the HTTP content type (e.g.: "text/html").
        /// </summary>
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

        /// <summary>
        /// Contains the exact number of headers from the request.
        /// </summary>
        public Int32 HeaderCount
        {
            get { return Headers.Count; }
        }

        /// <summary>
        /// Contains all headers from the request and their corresponding values.
        /// </summary>
        public IDictionary<String, String> Headers
        {
            get; private set;
        }

        /// <summary>
        /// Checks if the request is a valid request or not.
        /// </summary>
        public Boolean IsValid
        {
            get; private set;
        }

        /// <summary>
        /// Contains the HTTP method used by the request.
        /// </summary>
        public String Method
        {
            get; private set;
        }

        /// <summary>
        /// Contains the request URL as Url object.
        /// </summary>
        public IUrl Url
        {
            get; private set;
        }

        /// <summary>
        /// Contains the used user agent (in other words: the browser (client) used by the user).
        /// </summary>
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
