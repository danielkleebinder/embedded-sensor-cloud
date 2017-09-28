using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer {
    class Request : IRequest {

        private Stream network;
        private IDictionary<string, string> headers;

        public Request() : this(null) { }

        public Request(Stream network) {
            this.network = network;
        }

        public Byte[] ContentBytes {
            get {
                throw new NotImplementedException();
            }
        }

        public Int32 ContentLength {
            get {
                throw new NotImplementedException();
            }
        }

        public Stream ContentStream {
            get {
                throw new NotImplementedException();
            }
        }

        public String ContentString {
            get {
                throw new NotImplementedException();
            }
        }

        public String ContentType {
            get {
                throw new NotImplementedException();
            }
        }

        public Int32 HeaderCount {
            get {
                throw new NotImplementedException();
            }
        }

        public IDictionary<String, String> Headers {
            get {
                throw new NotImplementedException();
            }
        }

        public Boolean IsValid {
            get {
                throw new NotImplementedException();
            }
        }

        public String Method {
            get {
                throw new NotImplementedException();
            }
        }

        public IUrl Url {
            get {
                throw new NotImplementedException();
            }
        }

        public String UserAgent {
            get {
                throw new NotImplementedException();
            }
        }
    }
}
