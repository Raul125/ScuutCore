namespace ScuutCore.Modules.Subclasses
{
    using System.Collections.Generic;
    using System.IO;
    using API.Interfaces;
    using DeclaredSubclasses.Scientist;
    using PluginAPI.Helpers;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float SpawnSubclassHintDuration { get; set; } = 5f;
        public string SubclassFolder { get; set; } = Path.Combine(Path.Combine(Paths.LocalPlugins.Plugins, "ScuutCore"), "Subclasses");
        public float MessageDelay { get; set; } = 1f;
        public bool LoadGuns { get; set; } = true;
        public bool GiveItemsToNonSubclasses { get; set; } = true;
        public float ChanceForItems { get; set; } = 69f;
        public List<ItemType> Items { get; set; } = new()
        {
            ItemType.Flashlight
        };
        public HeadResearcher HeadResearcher { get; set; } = new HeadResearcher();
    }
}