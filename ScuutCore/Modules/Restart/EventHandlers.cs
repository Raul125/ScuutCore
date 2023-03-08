namespace ScuutCore.Modules.Restart
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ScuutCore.API.Features;
    using ServerOutput;

    public sealed class EventHandlers : InstanceBasedEventHandler<Restart>
    {
        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            ServerStatic.StopNextRound = ServerStatic.NextRoundAction.Restart;
            ServerConsole.AddOutputEntry(new ExitActionRestartEntry());
        }
    }
}