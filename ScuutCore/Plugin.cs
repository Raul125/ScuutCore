namespace ScuutCore
{
    using System;
    using MEC;
    using System.Collections.Generic;
    using HarmonyLib;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;
    using ScuutCore.API.Loader;
    using PluginAPI.Core;
    using ScuutCore.API.Features;

    public class Plugin
    {
        public static Harmony Harmony { get; internal set; }
        public static Plugin Singleton { get; internal set; }
        public static List<CoroutineHandle> Coroutines = new();

        private static Random Rand;
        public static Random Random
        {
            get
            {
                if (Rand is null)
                    Rand = new Random();

                return Rand;
            }
        }

        [PluginConfig] public Config Config;

        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint("ScuutCore", "1.0.5", "ScuutCore", "Raul125")]
        public void LoadPlugin()
        {
            Singleton = this;

            FactoryManager.RegisterPlayerFactory(this, new ScuutPlayerFactory());

            Loader.InitModules();

            Harmony = new Harmony("scuutcore.raul." + DateTime.Now.Ticks);
            Harmony.PatchAll();

            EventManager.RegisterEvents<EventHandlers>(this);
        }

        [PluginUnload]
        public void OnDisabled()
        {
            Loader.StopModules();

            EventManager.UnregisterEvents<EventHandlers>(this);

            Singleton = null;
            Harmony.UnpatchAll();
        }
    }
}