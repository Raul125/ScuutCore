namespace ScuutCore
{
    using System;
    using System.Collections.Generic;
    using API.Features;
    using API.Loader;
    using HarmonyLib;
    using MEC;
    using Patches;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;

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
        [PluginEntryPoint("ScuutCore", "1.0.6", "ScuutCore", "Raul125")]
        public void LoadPlugin()
        {
            Singleton = this;

            FactoryManager.RegisterPlayerFactory(this, new ScuutPlayerFactory());

            Loader.InitModules();

            Harmony = new Harmony("scuutcore.raul." + DateTime.Now.Ticks);
            Harmony.PatchAll();

            EventManager.RegisterEvents<EventHandlers>(this);

            LockerPatch.ResetChance();
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