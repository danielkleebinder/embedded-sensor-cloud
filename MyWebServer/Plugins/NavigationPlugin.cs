﻿using System;
using BIF.SWE1.Interfaces;
using System.IO;
using System.Xml;
using System.Text;

namespace MyWebServer.Plugins
{
    class NavigationPlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url == null || req.Url.Segments.Length < 1)
            {
                return 0.0f;
            }
            if (req.Url.Segments[0] == "navi")
            {
                if (HTTP.METHOD_GET.Equals(req.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    return 0.8f;
                }
                if (HTTP.METHOD_POST.Equals(req.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    return 1.0f;
                }
                return 0.5f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            IResponse result = new Response();
            result.StatusCode = 200;

            // Parse the street parameter
            string street = null;
            if (req.Url.Parameter.ContainsKey("street"))
            {
                street = req.Url.Parameter["street"];
            }
            else if (req.ContentString != null && req.ContentString.Contains("street="))
            {
                string[] splits = req.ContentString.Split('=');
                if (splits.Length > 1)
                {
                    street = splits[1];
                }
            }


            //TODO: Use XML Response Here
            if (string.IsNullOrEmpty(street))
            {
                result.SetContent("Bitte geben Sie eine Anfrage ein");
                return result;
            }
            else
            {
                // Read XML using SAX and build the result
                StringBuilder content = new StringBuilder();
                content.AppendLine("Orte gefunden");
                /*using (XmlReader reader = XmlReader.Create("./streetmap/austria.osm.pbf"))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }
                        if (reader.Name == street)
                        {
                            content.AppendLine();
                        }
                    }
                }*/

                // Finish Response
                result.SetContent(content.ToString());
            }

            return result;
        }
    }
}