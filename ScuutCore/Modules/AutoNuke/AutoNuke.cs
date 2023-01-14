﻿using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.AutoNuke
{
    using ScuutCore.API.Features;

    public class AutoNuke : Module<Config>
    {
        public override string Name { get; } = "AutoNuke";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(this, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}