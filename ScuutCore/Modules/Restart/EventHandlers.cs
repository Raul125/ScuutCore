namespace ScuutCore.Modules.Restart
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ServerOutput;

    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            ServerStatic.StopNextRound = ServerStatic.NextRoundAction.Restart;
            ServerConsole.AddOutputEntry(default(ExitActionRestartEntry));
        }
    }
}