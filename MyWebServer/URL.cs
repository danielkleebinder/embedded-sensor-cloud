using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer {
    public class Url : IUrl {
        private IDictionary<string, string> parameters;
        private string raw;
        private string path = string.Empty;
        private string fragment = string.Empty;
        private string fileName = string.Empty;
        private string extension = string.Empty;
        private string[] segments;

        public Url() : this(null) { }

        public Url(string raw) {
            CompileURL(raw);
        }

        /// <summary>
        /// Compiles the given raw URL string and extracts all important informations.
        /// </summary>
        private void CompileURL(string raw) {
            this.raw = raw;
            parameters = new Dictionary<string, string>();
            if (raw == null) {
                return;
            }

            this.path = raw;

            // Split path and parameters
            string[] splits = raw.Split('?');
            if (splits.Length > 0) {
                path = splits[0];
            }

            // Parse segments and fragments
            ParseSegments(path);
            ParseFragments(path);

            // Add parameters to parameter dictionary
            if (splits.Length > 1) {
                ParseParameters(splits[1]);
            }

            // Parse file name if existing
            ParseFileName();
        }


        /// <summary>
        /// Parses the parameters list of the standard URL format
        /// </summary>
        private void ParseParameters(string paramString) {
            string[] parameterList = paramString.Split('&');
            string[] result;
            foreach (string param in parameterList) {
                result = param.Split('=');
                if (result.Length > 1) {
                    parameters.Add(result[0], result[1]);
                } else if (result.Length > 0) {
                    parameters.Add(result[0], "");
                }
            }
        }


        /// <summary>
        /// Parses directory segments (e.g. /my/directory/file.x). All segments
        /// of the path will be stored separately
        /// </summary>
        private void ParseSegments(string path) {
            string[] segSplits = path.Split('/');
            if (segSplits.Length > 0) {
                segments = new string[segSplits.Length - 1];
                for (int i = 0; i < segSplits.Length - 1; i++) {
                    segments[i] = segSplits[i + 1];
                }
            }
        }

        /// <summary>
        /// Parses the fragment (if existing). Fragments are directories added
        /// to the end of the URL using the '#' character.
        /// </summary>
        private void ParseFragments(string path) {
            string[] fragSplit = path.Split('#');
            if (fragSplit.Length > 1) {
                fragment = fragSplit[1];
                path = fragSplit[0];
            }
        }

        /// <summary>
        /// Parses the file name. The file name is the last segment in the segments
        /// array which contains a '.' character. This method also extracts the file
        /// extension.
        /// </summary>
        private void ParseFileName() {
            // Parse file name
            string last = segments[segments.Length - 1];
            if (last != null && last.Contains('.')) {
                fileName = last;
            }

            // Parse file extension
            string[] extSplits = fileName.Split('.');
            if (extSplits.Length >= 1) {
                extension = "." + extSplits[extSplits.Length - 1];
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
            get {
                return extension;
            }
        }

        public string FileName {
            get {
                return fileName;
            }
        }

        public string Fragment {
            get { return fragment; }
        }

        public string[] Segments {
            get { return segments; }
        }
    }
}
