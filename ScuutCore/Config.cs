namespace ScuutCore.Main
{
    using PluginAPI.Helpers;
    using System.IO;

    public class Config
    {
        public string ConfigsFolder = Path.Combine(Paths.Configs, "ScuutCore");
        public bool Debug { get; set; } = false;
    }
}
