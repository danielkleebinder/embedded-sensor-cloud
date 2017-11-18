using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MyWebServer
{
    public sealed class HTTP
    {
        // Contains all default web server documents
        public static readonly string[] DEFAULT_DOCUMENTS =
        {
            "", "index.html", "index.htm",
            "default.html", "default.htm",
            "home.html", "home.htm"
        };

        public static readonly IDictionary<string, string> MIME_TYPE_MAPPING = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "asf", "video/x-ms-asf"},
            { "asx", "video/x-ms-asf"},
            { "avi", "video/x-msvideo"},
            {"bin", "application/octet-stream"},
            {"cco", "application/x-cocoa"},
            {"crt", "application/x-x509-ca-cert"},
            {"css", "text/css"},
            {"deb", "application/octet-stream"},
            {"der", "application/x-x509-ca-cert"},
            {"dll", "application/octet-stream"},
            { "dmg", "application/octet-stream"},
            { "ear", "application/java-archive"},
            { "eot", "application/octet-stream"},
            { "exe", "application/octet-stream"},
            { "flv", "video/x-flv"},
            { "gif", "image/gif"},
            { "hqx", "application/mac-binhex40"},
            {"htc", "text/x-component"},
            {"htm", "text/html"},
            {"html", "text/html"},
            {"ico", "image/x-icon"},
            {"img", "application/octet-stream"},
            {"iso", "application/octet-stream"},
            {"jar", "application/java-archive"},
            {"jardiff", "application/x-java-archive-diff"},
            {"jng", "image/x-jng"},
            {"jnlp", "application/x-java-jnlp-file"},
            {"jpeg", "image/jpeg"},
            {"jpg", "image/jpeg"},
            {"js", "application/x-javascript"},
            {"mml", "text/mathml"},
            {"mng", "video/x-mng"},
            {"mov", "video/quicktime"},
            {"mp3", "audio/mpeg"},
            {"mpeg", "video/mpeg"},
            {"mpg", "video/mpeg"},
            {"msi", "application/octet-stream"},
            {"msm", "application/octet-stream"},
            {"msp", "application/octet-stream"},
            {"pdb", "application/x-pilot"},
            {"pdf", "application/pdf"},
            {"pem", "application/x-x509-ca-cert"},
            {"pl", "application/x-perl"},
            {"pm", "application/x-perl"},
            {"png", "image/png"},
            {"prc", "application/x-pilot"},
            {"ra", "audio/x-realaudio"},
            {"rar", "application/x-rar-compressed"},
            {"rpm", "application/x-redhat-package-manager"},
            {"rss", "text/xml"},
            {"run", "application/x-makeself"},
            {"sea", "application/x-sea"},
            {"shtml", "text/html"},
            {"sit", "application/x-stuffit"},
            {"swf", "application/x-shockwave-flash"},
            {"tcl", "application/x-tcl"},
            {"tk", "application/x-tcl"},
            {"txt", "text/plain"},
            {"war", "application/java-archive"},
            {"wbmp", "image/vnd.wap.wbmp"},
            {"wmv", "video/x-ms-wmv"},
            {"xml", "text/xml"},
            {"xpi", "application/x-xpinstall"},
            {"zip", "application/zip"}
        };

        // Basic HTTP stuff
        public static readonly string HTTP_PROTOCOL = "HTTP/";
        public static readonly string HTTP_PROTOCOL_LC = HTTP_PROTOCOL.ToLower();
        public static readonly string USER_AGENT = "User-Agent";
        public static readonly string USER_AGENT_LC = USER_AGENT.ToLower();
        public static readonly string LOCATION = "Location";
        public static readonly string LOCATION_LC = LOCATION.ToLower();
        public static readonly string CONTENT_LENGTH = "Content-Length";
        public static readonly string CONTENT_LENGTH_LC = CONTENT_LENGTH.ToLower();
        public static readonly string CONTENT_LANGUAGE = "Content-Language";
        public static readonly string CONTENT_LANGUAGE_LC = CONTENT_LANGUAGE.ToLower();
        public static readonly string CONTENT_LANGUAGE_EN = "en";
        public static readonly string CONTENT_LANGUAGE_DE = "de";
        public static readonly string CONTENT_ENCODING = "Content-Encoding";
        public static readonly string CONTENT_ENCODING_LC = CONTENT_ENCODING.ToLower();
        public static readonly string CONTENT_ENCODING_UTF8 = "utf-8";
        public static readonly string CONTENT_ENCODING_ASCII = "ascii";

        // Content Types
        public static readonly string CONTENT_TYPE = "Content-Type";
        public static readonly string CONTENT_TYPE_LC = CONTENT_TYPE.ToLower();
        public static readonly string CONTENT_TYPE_TEXT_XML = "text/xml";
        public static readonly string CONTENT_TYPE_TEXT_HTML = "text/html";
        public static readonly string CONTENT_TYPE_TEXT_JAVASCRIPT = "text/javascript";
        public static readonly string CONTENT_TYPE_TEXT_CSS = "text/css";
        public static readonly string CONTENT_TYPE_TEXT_PLAIN = "text/plain";
        public static readonly string CONTENT_TYPE_APPLICATION_PDF = "application/pdf";
        public static readonly string CONTENT_TYPE_IMAGE_JPG = "image/jpg";
        public static readonly string CONTENT_TYPE_IMAGE_PNG = "image/png";
        public static readonly string CONTENT_TYPE_IMAGE_GIF = "image/gif";

        // Connection Types
        public static readonly string CONNECTION = "Connection";
        public static readonly string CONNECTION_LC = CONNECTION.ToLower();
        public static readonly string CONNECTION_KEEP_ALIVE = "Keep-Alive";
        public static readonly string CONNECTION_KEEP_ALIVE_LC = CONNECTION_KEEP_ALIVE.ToLower();
        public static readonly string CONNECTION_CLOSED = "Closed";
        public static readonly string CONNECTION_CLOSED_LC = CONNECTION_CLOSED.ToLower();

        public static readonly string METHOD_GET = "GET";
        public static readonly string METHOD_POST = "POST";


        /// <summary>
        /// Returns the given content type combined with the given charset.
        /// </summary>
        public static string ContentTypeEncoding(string contentType, string encoding)
        {
            StringBuilder result = new StringBuilder();
            result.Append(contentType);
            result.Append("; charset=");
            result.Append(encoding);
            return result.ToString();
        }

        /// <summary>
        /// Returns the corresponding MIME type for the given file extension.
        /// </summary>
        public static string MimeTypeFromExtension(string extension)
        {
            // Return "text/plain" is no extension is specified
            if (extension == null)
            {
                return CONTENT_TYPE_TEXT_PLAIN;
            }

            // Remove first dot in extension
            if (extension.StartsWith("."))
            {
                extension = new Regex(Regex.Escape("."))
                    .Replace(extension, "", 1);
            }

            // Check if mime type exists, otherwise return "text/plain"
            if (!MIME_TYPE_MAPPING.ContainsKey(extension))
            {
                return CONTENT_TYPE_TEXT_PLAIN;
            }
            return MIME_TYPE_MAPPING[extension];
        }
    }
}
