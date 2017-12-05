using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    /// <summary>
    /// The Url class supports parsing of URLs. It is a useful tool to split and analyze
    /// URLs in general.
    /// </summary>
    public class Url : IUrl
    {
        private IDictionary<string, string> parameters;
        private string raw;
        private string path = string.Empty;
        private string fragment = string.Empty;
        private string fileName = string.Empty;
        private string extension = string.Empty;
        private string[] segments;

        /// <summary>
        /// Creates a new instance of "Url" with the given raw URL string.
        /// </summary>
        /// <param name="raw">Raw URL string.</param>
        public Url(string raw)
        {
            CompileURL(HttpUtility.UrlDecode(raw));
        }

        /// <summary>
        /// Compiles the given raw URL string and extracts all important informations.
        /// </summary>
        /// <param name="raw">Raw URL string.</param>
        private void CompileURL(string raw)
        {
            this.raw = raw;
            parameters = new Dictionary<string, string>();
            if (raw == null)
            {
                return;
            }

            this.path = raw;

            // Split path and parameters
            string[] splits = raw.Split('?');
            if (splits.Length > 0)
            {
                path = splits[0];
            }

            // Parse segments and fragments
            ParseSegments(path);
            string[] fragSplit = path.Split('#');
            if (fragSplit.Length > 1)
            {
                fragment = fragSplit[1];
                path = fragSplit[0];
            }

            // Add parameters to parameter dictionary
            if (splits.Length > 1)
            {
                ParseParameters(splits[1]);
            }

            // Parse file name if existing
            ParseFileName();
        }


        /// <summary>
        /// Parses the parameter list of the standard URL format.
        /// </summary>
        /// <param name="paramString">Parameter string.</param>
        private void ParseParameters(string paramString)
        {
            string[] parameterList = paramString.Split('&');
            string[] result;
            foreach (string param in parameterList)
            {
                result = param.Split('=');
                if (result.Length > 1)
                {
                    parameters.Add(result[0], result[1]);
                }
                else if (result.Length > 0)
                {
                    parameters.Add(result[0], "");
                }
            }
        }


        /// <summary>
        /// Parses directory segments (e.g. /my/directory/file.x). All segments
        /// of the path will be stored separately.
        /// </summary>
        /// <param name="path">URL path.</param>
        private void ParseSegments(string path)
        {
            segments = path?.Split(new char[] { '/', '\\' }).Skip(1).ToArray() ?? new String[] { };
        }

        /// <summary>
        /// Parses the file name. The file name is the last segment in the segments
        /// array which contains a '.' character. This method also extracts the file
        /// extension.
        /// </summary>
        private void ParseFileName()
        {
            // Parse file name
            string last = segments.Last();
            if (last != null && last.Contains('.'))
            {
                fileName = last;
            }

            // Parse file extension
            string[] extSplits = fileName.Split('.');
            if (extSplits.Length >= 1)
            {
                extension = "." + extSplits.Last();
            }
        }

        /// <summary>
        /// Contains all URL parameters.
        /// </summary>
        public IDictionary<string, string> Parameter
        {
            get { return parameters; }
        }

        /// <summary>
        /// Contains the exact number of available parameters.
        /// </summary>
        public int ParameterCount
        {
            get { return parameters.Count; }
        }

        /// <summary>
        /// Contains the path to a given resource which was specified in the URL.
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// Returns the raw URL string.
        /// </summary>
        public string RawUrl
        {
            get { return raw; }
        }

        /// <summary>
        /// Contains the file extension if one exists. Otherwise it will be null.
        /// </summary>
        public string Extension
        {
            get { return extension; }
        }

        /// <summary>
        /// Returns the raw file name of the resource specified in the URL.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// Contains the fragments name in the URL (google.com/shop#fragment).
        /// </summary>
        public string Fragment
        {
            get { return fragment; }
        }

        /// <summary>
        /// Contains all segments specified in the URL.
        /// </summary>
        public string[] Segments
        {
            get { return segments; }
        }
    }
}
