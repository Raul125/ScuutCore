namespace ScuutCore.Modules.AdminTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ScuutCore.API.Features;
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<AdminTools>
    {

        public static readonly List<Jailed> JailedPlayers = new List<Jailed>();

        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnPlayerVerified(Player player)
        {
            if (JailedPlayers.Any(j => j.UserId == player.UserId))
                Plugin.Coroutines.Add(Timing.RunCoroutine(DoJail(player, true)));
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(global::RoundSummary.LeadingTeam leadingTeam)
        {
            foreach (Jailed jail in JailedPlayers)
            {
                if (jail.CurrentRound)
                    jail.CurrentRound = false;
            }
        }

        // Jail Part
        public static IEnumerator<float> DoJail(Player player, bool skipadd = false)
        {
            var items = new List<ItemType>();
            var ammo = new Dictionary<ItemType, ushort>();
            foreach (var kvp in player.ReferenceHub.inventory.UserInventory.ReserveAmmo)
                ammo.Add(kvp.Key, kvp.Value);

            foreach (var item in player.Items)
                items.Add(item.ItemTypeId);

            if (!skipadd)
            {
                JailedPlayers.Add(new Jailed
                {
                    Health = player.Health,
                    Position = player.Position,
                    Items = items,
                    Name = player.Nickname,
                    Role = player.Role,
                    UserId = player.UserId,
                    CurrentRound = true,
                    Ammo = ammo
                });
            }

            if (player.IsOverwatchEnabled)
                player.IsOverwatchEnabled = false;

            yield return Timing.WaitForSeconds(1f);
            player.ClearInventory(false);
            player.SetRole(RoleTypeId.Tutorial);
            yield return Timing.WaitForSeconds(1f);
            player.IsGodModeEnabled = true;
            player.Position = new Vector3(38.000f, 1014.112f, -32.000f);
        }

        public static IEnumerator<float> DoUnJail(Player player)
        {
            var jail = JailedPlayers.Find(j => j.UserId == player.UserId);
            if (jail.CurrentRound)
            {
                player.SetRole(jail.Role);
                yield return Timing.WaitForSeconds(0.5f);
                try
                {
                    player.IsGodModeEnabled = false;
                    foreach (var item in jail.Items)
                        player.AddItem(item);

                    player.Health = jail.Health;
                    player.Position = jail.Position;
                    foreach (var kvp in jail.Ammo)
                        player.AddAmmo(kvp.Key, kvp.Value);
                }
                catch (Exception e)
                {
                    Log.Error($"{nameof(DoUnJail)}: {e}");
                }
            }
            else
            {
                player.SetRole(RoleTypeId.Spectator);
            }

            JailedPlayers.Remove(jail);
        }

        public sealed class Jailed
        {
            public string UserId;
            public string Name;
            public List<ItemType> Items;
            public RoleTypeId Role;
            public Vector3 Position;
            public float Health;
            public Dictionary<ItemType, ushort> Ammo;
            public bool CurrentRound;
        }
    }
}