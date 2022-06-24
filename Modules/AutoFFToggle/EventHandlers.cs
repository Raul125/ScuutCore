﻿using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace ScuutCore.Modules.AutoFFToggle
{
    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        public void OnWaitingForPlayers()
        {
            GameCore.Console.singleton.TypeCommand("/setconfig friendly_fire false", null);
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            GameCore.Console.singleton.TypeCommand("/setconfig friendly_fire true", null);
        }
    }
}