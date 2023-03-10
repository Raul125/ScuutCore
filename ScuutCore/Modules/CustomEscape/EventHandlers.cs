namespace ScuutCore.Modules.CustomEscape
{
    using System.Collections.Generic;
    using ScuutCore.API.Features;
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public sealed class EventHandlers : InstanceBasedEventHandler<CustomEscape>
    {
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
            Plugin.Coroutines.Add(Timing.RunCoroutine(BetterDisarm()));
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        public bool OnEscaping(Player player, RoleTypeId newRole) => !Module.Config.CuffedRoleConversions.ContainsKey(player.Role);

        private IEnumerator<float> BetterDisarm()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(.5f);

                foreach (Player player in Player.GetPlayers())
                {
                    if (!player.IsDisarmed || !Module.Config.CuffedRoleConversions.TryGetValue(player.Role, out var role) || (Escape.WorldPos - player.Position).sqrMagnitude > Escape.RadiusSqr)
                            continue;

                    player.SetRole(role);
                }
            }
        }
    }
}