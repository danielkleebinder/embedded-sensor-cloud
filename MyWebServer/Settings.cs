using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer {
    class Settings {

        public static readonly IDictionary<Int32, string> STATUS_CODES = new Dictionary<Int32, string>();

        static Settings() {
            STATUS_CODES.Add(100, "Continue");
            STATUS_CODES.Add(101, "Switching Protocols");
            STATUS_CODES.Add(102, "Processing");
            STATUS_CODES.Add(200, "OK");
            STATUS_CODES.Add(201, "Created");
            STATUS_CODES.Add(202, "Accepted");
            STATUS_CODES.Add(203, "Non-Authoritative Information");
            STATUS_CODES.Add(204, "No Content");
            STATUS_CODES.Add(206, "Partial Content");
            STATUS_CODES.Add(207, "Multi-Status");
            STATUS_CODES.Add(208, "Already Reported");
            STATUS_CODES.Add(226, "IM Used");
            STATUS_CODES.Add(300, "Multiple Choices");
            STATUS_CODES.Add(301, "Moved Permanently");
            STATUS_CODES.Add(302, "Found (Moved Temporarily)");
            STATUS_CODES.Add(303, "See Other");
            STATUS_CODES.Add(304, "Not Modified");
            STATUS_CODES.Add(305, "Use Proxy");
            STATUS_CODES.Add(400, "Bad Request");
            STATUS_CODES.Add(401, "Unauthorized");
            STATUS_CODES.Add(402, "Payment Required");
            STATUS_CODES.Add(403, "Forbidden");
            STATUS_CODES.Add(404, "Not Found");
            STATUS_CODES.Add(405, "Method Not Allowed");
            STATUS_CODES.Add(406, "Not Acceptable");
            STATUS_CODES.Add(500, "Internal Server Error");
            STATUS_CODES.Add(501, "Not Implemented");
            STATUS_CODES.Add(502, "Bad Gateway");
            STATUS_CODES.Add(503, "Service Unavailable");
            STATUS_CODES.Add(504, "Gateway Time-out");
        }
    }
}
