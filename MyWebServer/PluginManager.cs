using System;
using System.Collections.Generic;
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
            // Check if the plugin directory does even exist
            string pluginPath = AppContext.Current.PluginDirectory;
            if (!Directory.Exists(pluginPath)) {
                Directory.CreateDirectory(pluginPath);
                return;
            }

            // Scan the plugin directory for all available plugins and try to load them
            foreach (string file in Directory.GetFiles(pluginPath)) {
                if (!file.EndsWith(".dll")) {
                    continue;
                }

                // Load DLL assembly and scan for the "IPlugin" interface
                Assembly pluginDLL = Assembly.LoadFrom(Path.Combine(AppContext.Current.PluginDirectory, file));
                Type[] pluginTypes = pluginDLL.GetTypes();
                foreach (Type pluginType in pluginTypes) {
                    if (pluginType.GetInterface("BIF.SWE1.Interfaces.IPlugin") == null) {
                        continue;
                    }

                    // Create an instance of the plugin and add it to the plugin manager
                    Add(Activator.CreateInstance(pluginType) as IPlugin);
                }
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