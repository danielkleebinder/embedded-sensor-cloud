using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace Uebungen
{
    public class UEB6 : IUEB6
    {
        public void HelloWorld()
        {
        }

        public IPluginManager GetPluginManager()
        {
            return new PluginManager();
        }

        public IRequest GetRequest(System.IO.Stream network)
        {
            return new Request(network);
        }

        public string GetNaviUrl()
        {
            return "/navi";
        }

        public IPlugin GetNavigationPlugin()
        {
            return new MyWebServer.Plugins.NavigationPlugin();
        }

        public IPlugin GetTemperaturePlugin()
        {
            return new MyWebServer.Plugins.TemperaturePlugin();
        }

        public string GetTemperatureRestUrl(DateTime from, DateTime until)
        {
            StringBuilder result = new StringBuilder();
            result.Append("/temp?");
            result.Append("type=").Append("rest").Append("&");
            result.Append("from=").Append(from.ToString()).Append("&");
            result.Append("until=").Append(until.ToString());
            return result.ToString();
        }

        public string GetTemperatureUrl(DateTime from, DateTime until)
        {
            StringBuilder result = new StringBuilder();
            result.Append("/temp?");
            result.Append("from=").Append(from.ToString()).Append("&");
            result.Append("until=").Append(until.ToString());
            return result.ToString();
        }

        public IPlugin GetToLowerPlugin()
        {
            return new MyWebServer.Plugins.LowerPlugin();
        }

        public string GetToLowerUrl()
        {
            return "/";
        }
    }
}
