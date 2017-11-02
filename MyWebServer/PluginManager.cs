using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {

        public PluginManager()
        {
            Add(new Plugins.TestPlugin());
            Add(new Plugins.StaticFilePlugin());
            Add(new Plugins.LowerPlugin());
            Add(new Plugins.TemperaturePlugin());
            Add(new Plugins.NavigationPlugin());

            // Loads all dynamic plugins
            LoadDynamicPlugins();
        }

        private void LoadDynamicPlugins()
        {
            Assembly pluginDLL = Assembly.LoadFrom(Path.Combine(AppContext.Current.PluginDirectory, "GooglePlugin.dll"));
            Type[] pluginTypes = pluginDLL.GetTypes();
            foreach (Type pluginType in pluginTypes) {
                if (pluginType.GetInterface("BIF.SWE1.Interfaces.IPlugin") == null) {
                    continue;
                }
                Add(Activator.CreateInstance(pluginType) as IPlugin);
            }
        }

        public IEnumerable<IPlugin> Plugins { get; } = new List<IPlugin>();

        public void Add(String plugin)
        {
            // Parse plugin string
            if (string.IsNullOrEmpty(plugin)) {
                throw new ArgumentNullException("Plugin is not allowed to be empty or null");
            }

            // Assemble object from class name
            Type type = Type.GetType(plugin);
            IPlugin result = (IPlugin) Activator.CreateInstance(type);

            // Add plugin
            Add(result);
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