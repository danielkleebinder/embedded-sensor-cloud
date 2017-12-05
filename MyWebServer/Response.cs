using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MyWebServer
{
    /// <summary>
    /// A class which handles HTTP responses.
    /// </summary>
    public class Response : IResponse
    {
        private Int32 statusCode;
        private Byte[] contentBytes;

        /// <summary>
        /// Creates a new instance of "Reponse" using the standard server name.
        /// </summary>
        public Response()
        {
            Headers = new Dictionary<string, string>();
            ServerHeader = "BIF-SWE1-Server";
        }

        /// <summary>
        /// Returns the content length or 0 if no content is set yet.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the content type of the response.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// A specialized implementation may throw a InvalidOperationException
        /// when the content type is set by the implementation.
        /// </exception>
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

        /// <summary>
        /// Returns a writable dictionary of the response headers. Never returns null.
        /// </summary>
        public IDictionary<String, String> Headers
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the Server response header. Defaults to "BIF-SWE1-Server".
        /// </summary>
        public String ServerHeader
        {
            get; set;
        }

        /// <summary>
        /// Returns the status code as string (e.g.: (200 OK)).
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current status code. An Exceptions is thrown, if no status code was set.
        /// </summary>
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

        /// <summary>
        /// Adds or replaces a response header in the headers dictionary.
        /// </summary>
        /// <param name="header">Header name.</param>
        /// <param name="value">Value of the header.</param>
        public void AddHeader(String header, String value)
        {
            Headers[header] = value;
        }

        /// <summary>
        /// Sends the response to the network stream.
        /// </summary>
        /// <param name="network">Network stream.</param>
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
                try
                {
                    BinaryWriter contentWriter = new BinaryWriter(network);
                    contentWriter.Write(contentBytes);
                    contentWriter.Flush();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Sets the stream as content.
        /// </summary>
        /// <param name="stream">Stream content.</param>
        public void SetContent(Stream stream)
        {
            // Convert stream to byte array
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                SetContent(ms.ToArray());
            }
        }

        /// <summary>
        /// Sets a byte[] as content.
        /// </summary>
        /// <param name="content">Byte array content.</param>
        public void SetContent(Byte[] content)
        {
            this.contentBytes = content;
            Headers[HTTP.CONTENT_LENGTH_LC] = content.Length.ToString();
        }

        /// <summary>
        /// Sets a string content. The content will be encoded in UTF-8.
        /// </summary>
        /// <param name="content">String content.</param>
        public void SetContent(String content)
        {
            // Convert string to bytes
            SetContent(Encoding.UTF8.GetBytes(content));
        }
    }
}
