namespace ScuutCore.API.Features
{
    using Interfaces;
    using PluginAPI.Core;

    public abstract class Module<TModuleConfig> : IModule<TModuleConfig> where TModuleConfig : IModuleConfig, new()
    {

        public abstract string Name { get; }

        public TModuleConfig Config { get; set; } = new TModuleConfig();

        public virtual void OnEnabled() => Log.Debug($"Module {Name} Enabled");

        public virtual void OnDisabled() => Log.Debug($"Module {Name} Disabled");

        // Don't needed
        public int CompareTo(IModule<IModuleConfig> other) => 0;
    }
}