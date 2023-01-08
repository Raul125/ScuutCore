namespace ScuutCore.Modules.AutoFFToggle
{
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            Server.RunCommand("/setconfig friendly_fire false");
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnded(global::RoundSummary.LeadingTeam leadingTeam)
        {
            Server.RunCommand("/setconfig friendly_fire true");
        }
    }
}