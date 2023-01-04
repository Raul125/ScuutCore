namespace ScuutCore.Main
{
    using PluginAPI.Helpers;
    using System.Collections.Generic;
    using System.IO;

    public class Config
    {
        public string ConfigsFolder = Path.Combine(Paths.GlobalPlugins.Plugins, "ScuutCore");
        public bool Debug { get; set; } = false;

        public Dictionary<string, ushort> ServerCommand { get; set; } = new Dictionary<string, ushort>()
        {
            { "sv1", 25570 },
            { "sv2", 25571 }
        };
    }
}
