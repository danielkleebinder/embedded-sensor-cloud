using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {

        public PluginManager()
        {
            Add(new TestPlugin.TestPlugin());
        }

        public IEnumerable<IPlugin> Plugins { get; } = new List<IPlugin>();

        public void Add(String plugin)
        {
            // Parse plugin string
            if (string.IsNullOrEmpty(plugin))
            {
                throw new ArgumentNullException("Plugin is not allowed to be empty or null");
            }
            string[] splits = plugin.Trim().Replace(" ", "").Split(',');

            if (splits == null || splits.Length != 2)
            {
                throw new InvalidOperationException("Plugin must contain <class-path>, <name>");
            }

            // Extract class name and plugin name
            string className = splits[0];
            string name = splits[0];

            // Assemble object from class name
            Type type = Type.GetType(className);
            IPlugin result = (IPlugin)Activator.CreateInstance(type);

            // Add plugin
            Add(result);
        }

        public void Add(IPlugin plugin)
        {
            ((List<IPlugin>)Plugins).Add(plugin);
        }

        public void Clear()
        {
            ((List<IPlugin>)Plugins).Clear();
        }
    }
}