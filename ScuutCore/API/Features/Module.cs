using CommandSystem;
using Exiled.API.Features;
using System;

namespace ScuutCore.API
{
    public abstract class Module<TModuleConfig> : IModule<TModuleConfig> where TModuleConfig : IModuleConfig, new()
    {
        public Module()
        {

        }

        public virtual string Name { get; }

        public TModuleConfig Config { get; set; } = new TModuleConfig();

        public virtual void OnEnabled() => Log.Debug($"Module {Name} Enabled", Plugin.Singleton.Config.Debug);

        public virtual void OnDisabled() => Log.Debug($"Module {Name} Disabled", Plugin.Singleton.Config.Debug);

        // Don't needed
        public int CompareTo(IModule<IModuleConfig> other) => 0;
    }
}