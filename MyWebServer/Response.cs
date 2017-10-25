using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyWebServer
{
    class Response : IResponse
    {
        private Int32 statusCode;
        private String contentString;

        public Response()
        {
            Headers = new Dictionary<string, string>();
            ServerHeader = "BIF-SWE1-Server";
        }

        public Int32 ContentLength
        {
            get
            {
                if (contentString != null) {
                    return Encoding.UTF8.GetByteCount(contentString);
                }
                return 0;
            }
        }

        public String ContentType
        {
            get
            {
                if (!Headers.ContainsKey(Settings.CONTENT_TYPE)) {
                    return null;
                }
                return Headers[Settings.CONTENT_TYPE];
            }

            set
            {
                Headers[Settings.CONTENT_TYPE] = value;
            }
        }

        public IDictionary<String, String> Headers
        {
            get; private set;
        }

        public String ServerHeader
        {
            get; set;
        }

        public String Status
        {
            get
            {
                if (Settings.STATUS_CODES.ContainsKey(statusCode)) {
                    StringBuilder result = new StringBuilder(64);
                    result.Append(statusCode);
                    result.Append(" ");
                    result.Append(Settings.STATUS_CODES[statusCode]);
                    return result.ToString();
                }
                return null;
            }
        }

        public Int32 StatusCode
        {
            get
            {
                if (statusCode <= 0) {
                    throw new InvalidOperationException("Status Code was not set!");
                }
                return statusCode;
            }

            set
            {
                statusCode = value;
            }
        }

        public void AddHeader(String header, String value)
        {
            Headers[header] = value;
        }

        public void Send(Stream network)
        {
            if (!String.IsNullOrEmpty(ContentType) && ContentLength <= 0) {
                throw new InvalidOperationException("Sending a content type without content is not allowed");
            }

            // Write header and data
            StreamWriter writer = new StreamWriter(network, Encoding.UTF8);
            writer.WriteLine("HTTP/1.1 {0}", Status);
            writer.WriteLine("Server: {0}", ServerHeader);
            foreach (var item in Headers) {
                writer.WriteLine("{0}: {1}", item.Key, item.Value);
            }
            writer.WriteLine();
            if (contentString != null) {
                writer.WriteLine(contentString);
            }
            writer.Flush();
        }

        public void SetContent(Stream stream)
        {
            SetContent(new StreamReader(stream).ReadToEnd());
        }

        public void SetContent(Byte[] content)
        {
            SetContent(Encoding.UTF8.GetString(content));
        }

        public void SetContent(String content)
        {
            this.contentString = content;
            Headers[Settings.CONTENT_LENGTH] = Encoding.UTF8.GetByteCount(contentString).ToString();
        }
    }
}
