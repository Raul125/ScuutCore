namespace ScuutCore.Modules.Subclasses
{
    using System.IO;
    using PluginAPI.Helpers;
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float SpawnSubclassHintDuration { get; set; } = 5f;
        public string SubclassFolder { get; set; } = Path.Combine(Path.Combine(Paths.LocalPlugins.Plugins, "ScuutCore"), "Subclasses");
    }
}