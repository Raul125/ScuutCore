using Exiled.API.Features;
using System;
using MEC;
using System.Collections.Generic;

namespace ScuutCore
{
    public class Plugin : Plugin<Config.Config>
    {
        public override string Name { get; } = "ScuutCore";
        public override string Prefix { get; } = "scuutcore";
        public override string Author { get; } = "Raul125";
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);
        public override Version Version { get; } = new Version(1, 0, 3);

        // Static Part
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

        public EventHandlers.EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            API.Loader.InitModules();

            EventHandlers = new EventHandlers.EventHandlers();
            Exiled.Events.Handlers.Server.RestartingRound += EventHandlers.OnRoundRestarting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            API.Loader.StopModules();

            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers.OnRoundRestarting;
            EventHandlers = null;

            Singleton = null;

            base.OnDisabled();
        }
    }
}