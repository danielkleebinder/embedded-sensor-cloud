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
        private Byte[] contentBytes;

        public Response()
        {
            Headers = new Dictionary<string, string>();
            ServerHeader = "BIF-SWE1-Server";
        }

        public Int32 ContentLength
        {
            get
            {
                if (contentBytes != null)
                {
                    return contentBytes.Length;
                }
                return 0;
            }
        }

        public String ContentType
        {
            get
            {
                if (!Headers.ContainsKey(HTTP.CONTENT_TYPE))
                {
                    return null;
                }
                return Headers[HTTP.CONTENT_TYPE];
            }

            set
            {
                Headers[HTTP.CONTENT_TYPE] = value;
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
                if (Settings.STATUS_CODES.ContainsKey(statusCode))
                {
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
                if (statusCode <= 0)
                {
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
            if (!String.IsNullOrEmpty(ContentType) && ContentLength <= 0)
            {
                throw new InvalidOperationException("Sending a content type without content is not allowed");
            }

            // Write Header Data
            StreamWriter headerWriter = new StreamWriter(network, Encoding.ASCII);
            headerWriter.NewLine = "\r\n";
            headerWriter.WriteLine("HTTP/1.1 {0}", Status);
            headerWriter.WriteLine("Server: {0}", ServerHeader);
            foreach (var item in Headers)
            {
                headerWriter.WriteLine("{0}: {1}", item.Key, item.Value);
            }
            headerWriter.WriteLine();
            headerWriter.Flush();

            // Write Content Data
            if (contentBytes != null)
            {
                BinaryWriter contentWriter = new BinaryWriter(network);
                //contentWriter.NewLine = "\r\n";
                contentWriter.Write(contentBytes);
                contentWriter.Flush();
            }
        }

        public void SetContent(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                SetContent(ms.ToArray());
            }
        }

        public void SetContent(Byte[] content)
        {
            this.contentBytes = content;
            Headers[HTTP.CONTENT_LENGTH_LC] = content.Length.ToString();
        }

        public void SetContent(String content)
        {
            SetContent(Encoding.UTF8.GetBytes(content));
        }
    }
}
