using System;
using System.Collections.Generic;
using System.Reflection;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer
{
    /// <summary>
    /// The plugin manager implementation handles the loading and distribution of all
    /// plugins used by the web server. It can also load plugins dynamically from a
    /// given directory.
    /// </summary>
    public class PluginManager : IPluginManager
    {
        /// <summary>
        /// Creates a new instance of "PluginManager" and loads all plugins.
        /// </summary>
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

        /// <summary>
        /// Loads plugins dynamically from a given directory by using the reflection API.
        /// </summary>
        private void LoadDynamicPlugins()
        {
            // Check if the plugin directory does even exist
            string pluginPath = AppContext.Current.PluginDirectory;
            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
                return;
            }

            // Scan the plugin directory for all available plugins and try to load them
            foreach (string file in Directory.GetFiles(pluginPath))
            {
                if (!file.EndsWith(".dll"))
                {
                    continue;
                }

                // Load DLL assembly and scan for the "IPlugin" interface
                Assembly pluginDLL = Assembly.LoadFrom(Path.Combine(AppContext.Current.PluginDirectory, file));
                Type[] pluginTypes = pluginDLL.GetTypes();
                foreach (Type pluginType in pluginTypes)
                {
                    if (pluginType.GetInterface("BIF.SWE1.Interfaces.IPlugin") == null)
                    {
                        continue;
                    }

                    // Create an instance of the plugin and add it to the plugin manager
                    Add(Activator.CreateInstance(pluginType) as IPlugin);
                }
            }
        }

        /// <summary>
        /// Returns an enumerable which contains all plugins.
        /// </summary>
        public IEnumerable<IPlugin> Plugins { get; } = new List<IPlugin>();

        /// <summary>
        /// Adds a new plugin to the plugin manager using dynamic loading via the reflection API.
        /// </summary>
        /// <param name="plugin">Plugin name.</param>
        public void Add(String plugin)
        {
            // Parse plugin string
            if (string.IsNullOrEmpty(plugin))
            {
                throw new ArgumentNullException("Plugin is not allowed to be empty or null");
            }

            // Assemble object from class name
            Type type = Type.GetType(plugin);
            IPlugin result = (IPlugin) Activator.CreateInstance(type);

            // Add plugin
            Add(result);
        }

        /// <summary>
        /// Adds the given plugin to the plugin manager.
        /// </summary>
        /// <param name="plugin">Plugin.</param>
        public void Add(IPlugin plugin)
        {
            ((List<IPlugin>) Plugins).Add(plugin);
        }

        /// <summary>
        /// Removes all plugins from the plugin manager.
        /// </summary>
        public void Clear()
        {
            ((List<IPlugin>) Plugins).Clear();
        }
    }
}