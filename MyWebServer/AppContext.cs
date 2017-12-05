using System.IO;

namespace MyWebServer
{
    /// <summary>
    /// Singleton app context. Contains often used, but single instanced objects in a
    /// static context.
    /// </summary>
    public sealed class AppContext
    {
        private static AppContext _current;

        private AppContext() { }

        /// <summary>
        /// Returns the current context. Only one can exist.
        /// </summary>
        public static AppContext Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new AppContext();
                }
                return _current;
            }
        }

        /// <summary>
        /// Returns or sets the static files directory.
        /// </summary>
        public string StaticFileDirectory
        {
            get; set;
        }

        /// <summary>
        /// Returns the current working directory.
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }
        }

        /// <summary>
        /// Returns the plugin directory.
        /// </summary>
        public string PluginDirectory
        {
            get
            {
                return Path.Combine(WorkingDirectory, "plugins");
            }
        }
    }
}
