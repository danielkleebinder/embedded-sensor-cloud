using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {
        public IEnumerable<IPlugin> Plugins { get; } = new List<IPlugin>();

        public void Add(String plugin)
        {
            throw new NotImplementedException();
        }

        public void Add(IPlugin plugin)
        {
            ( (List<IPlugin>) Plugins ).Add(plugin);
        }

        public void Clear()
        {
            ( (List<IPlugin>) Plugins ).Clear();
        }
    }
}