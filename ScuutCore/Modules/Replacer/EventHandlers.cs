using Exiled.API.Enums;
using PluginAPI.Core;
using PluginAPI.Core.Roles;
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
            if (Round.IsEnded || Round.IsLobby || !Round.IsStarted || Round.ElapsedTime.TotalSeconds > replacer.Config.DontReplaceTime 
                || replacer.Config.DisallowedRolesToReplace.Contains(ev.Player.Role.Type))
                return;

            if (ev.Player.IsAlive)
            {
                bool isScp079 = ev.Player.Role.Is(out Scp079Role scp079);
                int level = 0;
                if (isScp079)
                    level = scp079.Level;

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
                        if (isScp079)
                        {

                            return;
                        }

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