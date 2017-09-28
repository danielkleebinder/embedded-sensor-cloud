using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer {
    public class Url : IUrl {
        private string raw;
        private string path;
        private IDictionary<string, string> parameters;

        public Url() : this(null) { }

        public Url(string raw) {
            compileURL(raw);
        }

        private void compileURL(string raw) {
            this.raw = raw;
            parameters = new Dictionary<string, string>();

            if (raw == null) {
                return;
            }

            // Split path and parameters
            string[] splits = raw.Split('?');

            // Set path
            if (splits.Length > 0) {
                path = splits[0];
            } else {
                path = raw;
            }

            // Add parameters to parameter dictionary
            if (splits.Length > 1) {
                string[] parameterList = splits[1].Split('&');
                string[] result;
                foreach (string param in parameterList) {
                    result = param.Split('=');
                    if (result.Length > 1) {
                        parameters.Add(result[0], result[1]);
                    }
                }
            }
        }

        public IDictionary<string, string> Parameter {
            get { return parameters; }
        }

        public int ParameterCount {
            get { return parameters.Count; }
        }

        public string Path {
            get { return path; }
        }

        public string RawUrl {
            get { return raw; }
        }

        public string Extension {
            get { throw new NotImplementedException(); }
        }

        public string FileName {
            get { throw new NotImplementedException(); }
        }

        public string Fragment {
            get { throw new NotImplementedException(); }
        }

        public string[] Segments {
            get { throw new NotImplementedException(); }
        }
    }
}
