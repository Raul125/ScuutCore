﻿using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScuutCore.Modules.AdminTools
{
    public class EventHandlers
    {
        private AdminTools admintools;
        public EventHandlers(AdminTools at)
        {
            admintools = at;
        }

		public static List<Jailed> JailedPlayers = new List<Jailed>();

		public void OnPlayerVerified(VerifiedEventArgs ev)
		{
			if (JailedPlayers.Any(j => j.Userid == ev.Player.UserId))
				Plugin.Coroutines.Add(Timing.RunCoroutine(DoJail(ev.Player, true)));
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
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
			List<Item> items = new List<Item>();
			Dictionary<AmmoType, ushort> ammo = new Dictionary<AmmoType, ushort>();
			foreach (KeyValuePair<ItemType, ushort> kvp in player.Ammo)
				ammo.Add(kvp.Key.GetAmmoType(), kvp.Value);

			foreach (Item item in player.Items)
				items.Add(item);

			if (!skipadd)
			{
				JailedPlayers.Add(new Jailed
				{
					Health = player.Health,
					Position = player.Position,
					Items = items,
					Name = player.Nickname,
					Role = player.Role,
					Userid = player.UserId,
					CurrentRound = true,
					Ammo = ammo
				});
			}

			if (player.IsOverwatchEnabled)
				player.IsOverwatchEnabled = false;

			yield return Timing.WaitForSeconds(1f);
			player.ClearInventory(false);
			player.SetRole(RoleType.Tutorial);
			player.Position = new Vector3(53f, 1020f, -44f);
		}

		public static IEnumerator<float> DoUnJail(Player player)
		{
			Jailed jail = JailedPlayers.Find(j => j.Userid == player.UserId);
			if (jail.CurrentRound)
			{
				player.SetRole(jail.Role, SpawnReason.ForceClass, true);
				yield return Timing.WaitForSeconds(0.5f);
				try
				{
					player.ResetInventory(jail.Items);
					player.Health = jail.Health;
					player.Position = jail.Position;
					foreach (KeyValuePair<AmmoType, ushort> kvp in jail.Ammo)
						player.Ammo[kvp.Key.GetItemType()] = kvp.Value;
				}
				catch (Exception e)
				{
					Log.Error($"{nameof(DoUnJail)}: {e}");
				}
			}
			else
			{
				player.SetRole(RoleType.Spectator);
			}

			JailedPlayers.Remove(jail);
		}

		public class Jailed
		{
			public string Userid;
			public string Name;
			public List<Item> Items;
			public RoleType Role;
			public Vector3 Position;
			public float Health;
			public Dictionary<AmmoType, ushort> Ammo;
			public bool CurrentRound;
		}
	}
}