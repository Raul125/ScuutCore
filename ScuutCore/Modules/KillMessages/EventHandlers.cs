namespace ScuutCore.Modules.KillMessages
{
    using API.Features;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    public sealed class EventHandlers : InstanceBasedEventHandler<KillMessages>
    {
        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker is null || player is null || player == attacker || global::RoundSummary.singleton._roundEnded)
                return;

            attacker.ReceiveHint(Module.Config.Message.Replace("{name}", player.Nickname), 4);
        }
    }
}