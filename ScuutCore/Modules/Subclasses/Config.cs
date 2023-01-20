namespace ScuutCore.Modules.Subclasses
{
    using System.Collections.Generic;
    using System.IO;
    using PlayerRoles;
    using PluginAPI.Helpers;
    using ScuutCore.API.Interfaces;
    using ScuutCore.Modules.Subclasses.Models;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float SpawnSubclassHintDuration { get; set; } = 5f;
        public string SubclassFolder { get; set; } = Path.Combine(Paths.Configs, "Subclasses");
    }
}