using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScuutCore.Modules.CustomEscape
{
    public class EventHandlers
    {
        private CustomEscape customEscape;
        public EventHandlers(CustomEscape c)
        {
            customEscape = c;
        }

        public Vector3 EscapeZone = Vector3.zero;

        public void OnRoundStarted()
        {
            Plugin.Coroutines.Add(Timing.RunCoroutine(BetterDisarm()));
        }

        public void OnEscaping(EscapingEventArgs ev)
        {
            if (customEscape.Config.CuffedRoleConversions.ContainsKey(ev.Player.Role.Type))
                ev.IsAllowed = false;
        }

        private IEnumerator<float> BetterDisarm()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1.5f);

                foreach (Player player in Player.List)
                {
                    if (EscapeZone == Vector3.zero)
                        EscapeZone = Escape.WorldPos;

                    if (!player.IsCuffed || (player.Role.Team != Team.ChaosInsurgency && player.Role.Team != Team.FoundationForces) || (EscapeZone - player.Position).sqrMagnitude > 400f)
                        continue;

                    if (customEscape.Config.CuffedRoleConversions.TryGetValue(player.Role.Type, out var role))
                    {
                        Plugin.Coroutines.Add(Timing.RunCoroutine(DropItems(player, player.Items.ToList())));
                        player.Role.Set(role);
                    }
                }
            }
        }

        private IEnumerator<float> DropItems(Player player, IEnumerable<Item> items)
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (Item item in items)
                item.Spawn(player.Position, default);
        }
    }
}