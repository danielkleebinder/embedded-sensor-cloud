﻿using System.IO;

namespace MyWebServer
{
    public sealed class AppContext
    {
        private static AppContext _current;

        private AppContext() { }

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

        public string StaticFileDirectory
        {
            get; set;
        }

        public string WorkingDirectory
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }
        }

        public string PluginDirectory
        {
            get
            {
                return Path.Combine(WorkingDirectory, "plugins");
            }
        }
    }
}
