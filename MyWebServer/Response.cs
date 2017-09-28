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

        private IDictionary<string, string> headers = new Dictionary<string, string>();
        private Int32 statusCode;

        public Int32 ContentLength
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

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<String, String> Headers
        {
            get
            {
                return headers;
            }
        }

        public String ServerHeader
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
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
                    result.Append(Settings.STATUS_CODES[statusCode].ToUpper());
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
            headers[header] = value;
        }

        public void Send(Stream network)
        {
            throw new NotImplementedException();
        }

        public void SetContent(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void SetContent(Byte[] content)
        {
            throw new NotImplementedException();
        }

        public void SetContent(String content)
        {
            throw new NotImplementedException();
        }
    }
}
