using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.KillMessages
{
    public class EventHandlers
    {
        private KillMessages killMessages;
        public EventHandlers(KillMessages btc)
        {
            killMessages = btc;
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker is null || player is null || player == attacker || global::RoundSummary.singleton._roundEnded)
                return;

            attacker.ReceiveHint(killMessages.Config.Message.Replace("{name}", player.Nickname), 4);
        }
    }
}