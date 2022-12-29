using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using ScuutCore.Modules.ScpSwap;
using System.Collections.Generic;
using System.Linq;

namespace ScuutCore.Modules.Replacer
{
    public class EventHandlers
    {
        private Replacer replacer;
        public EventHandlers(Replacer btc)
        {
            replacer = btc;
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (Round.IsEnded || Round.IsLobby || !Round.IsStarted)
                return;

            if (ev.Player.IsAlive && ev.Player.Role.Type != RoleTypeId.Tutorial)
            {
                UnityEngine.Vector3 oldPos = ev.Player.Position;
                RoleTypeId oldRole = ev.Player.Role.Type;
                float oldHealth = ev.Player.Health;
                List<ItemType> oldItems = new List<ItemType>();
                foreach (var item in ev.Player.Items)
                    oldItems.Add(item.Type);
                ev.Player.ClearInventory();

                Player randomSpec = Player.List.FirstOrDefault(x => x.Role.Type is RoleTypeId.Spectator && !x.IsOverwatchEnabled);
                if (randomSpec != null)
                {
                    ev.Player.Broadcast(replacer.Config.BroadCast);
                    ev.Player.Role.Set(oldRole);
                    Timing.CallDelayed(1f, () =>
                    {
                        ev.Player.Position = oldPos;
                        ev.Player.Health = oldHealth;
                        foreach (var item in oldItems)
                            ev.Player.AddItem(item);
                    });
                }
            }
        }
    }
}