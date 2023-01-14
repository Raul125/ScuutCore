namespace ScuutCore
{
    using System;
    using MEC;
    using System.Collections.Generic;
    using HarmonyLib;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;
    using ScuutCore.Main;

    public class Plugin
    {
        // Static Part
        public static Harmony Harmony { get; internal set; }
        public static Plugin Singleton { get; internal set; }
        public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        private static Random Rand;
        public static Random Random
        {
            get
            {
                if (Rand == null)
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
            API.Loader.InitModules();

            Harmony = new Harmony("scuutcore.raul." + DateTime.Now.Ticks);
            Harmony.PatchAll();

            EventManager.RegisterEvents<EventHandlers>(this);
        }

        [PluginUnload]
        public void OnDisabled()
        {
            API.Loader.StopModules();

            EventManager.UnregisterEvents<EventHandlers>(this);

            Singleton = null;
            Harmony?.UnpatchAll();
        }
    }
}