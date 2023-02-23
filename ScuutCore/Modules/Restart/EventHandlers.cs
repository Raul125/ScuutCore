namespace ScuutCore.Modules.Restart
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ServerOutput;

    public sealed class EventHandlers
    {
        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            ServerStatic.StopNextRound = ServerStatic.NextRoundAction.Restart;
            ServerConsole.AddOutputEntry(new ExitActionRestartEntry());
        }
    }
}