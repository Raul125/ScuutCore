﻿using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace ScuutCore.Modules.Scp1162
{
    public class EventHandlers
    {
        private Scp1162 scp1162;
        public EventHandlers(Scp1162 btc)
        {
            scp1162 = btc;
        }

        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            try
            {
                if (!ev.IsAllowed) 
                    return;

                if (Vector3.Distance(ev.Player.Position, Exiled.API.Extensions.RoleExtensions.GetRandomSpawnProperties(RoleType.Scp173).Item1) <= 8.2f)
                {
                    if (scp1162.Config.UseHints)
                        ev.Player.ShowHint(scp1162.Config.ItemDropMessage, scp1162.Config.ItemDropMessageDuration);
                    else
                        ev.Player.Broadcast(scp1162.Config.ItemDropMessageDuration, scp1162.Config.ItemDropMessage, Broadcast.BroadcastFlags.Normal, true);
                    ev.IsAllowed = false;
                    var oldItem = ev.Item.Base.ItemTypeId;
                    ev.Player.RemoveItem(ev.Item);
                    var newItemType = scp1162.Config.Chances[Random.Range(0, scp1162.Config.Chances.Count)];
                    var newItem = Item.Create(newItemType);
                    ev.Player.AddItem(newItem);
                    ev.Player.DropItem(newItem);
                }
            }
            catch
            {
                //Ignore, ev.Player.RemoveItem false positive.
            }
        }
    }
}