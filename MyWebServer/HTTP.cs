using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer
{
    public sealed class HTTP
    {
        // Basic HTTP stuff
        public static readonly string HTTP_PROTOCOL = "HTTP/";
        public static readonly string HTTP_PROTOCOL_LC = HTTP_PROTOCOL.ToLower();
        public static readonly string USER_AGENT = "User-Agent";
        public static readonly string USER_AGENT_LC = USER_AGENT.ToLower();
        public static readonly string CONTENT_LENGTH = "Content-Length";
        public static readonly string CONTENT_LENGTH_LC = CONTENT_LENGTH.ToLower();
        public static readonly string CONTENT_LANGUAGE = "Content-Language";
        public static readonly string CONTENT_LANGUAGE_LC = CONTENT_LANGUAGE.ToLower();

        // Content Types
        public static readonly string CONTENT_TYPE = "Content-Type";
        public static readonly string CONTENT_TYPE_LC = CONTENT_TYPE.ToLower();
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
    }
}
