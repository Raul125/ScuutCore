namespace EventManager
{
    using System;
    using EventManager.Api;
    using Exiled.API.Features;

    public class MainClass : Plugin<MainConfig>
    {
        public override string Name { get; } = "DonatorEventManager";
        public override string Prefix { get; } = "donator_event_manager";
        public override string Author { get; } = "Jesus-QC ($)";
        public override Version Version { get; } = new Version(1, 0, 0);

        public static MainClass Instance { get; internal set; }
        public EventHandlers EventHandlers { get; internal set; }
        
        public override void OnEnabled()
        {
            Instance = this;

            Database.Load();

            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnStartedRound;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnStartedRound;

            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}