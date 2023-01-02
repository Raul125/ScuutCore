using PluginAPI.Core;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.BetterLateSpawn
{
    public class EventHandlers
    {
        private BetterLateSpawn betterLateSpawn;
        public EventHandlers(BetterLateSpawn bls)
        {
            betterLateSpawn = bls;
            Chances = new float[] { betterLateSpawn.Config.ClassDChance, betterLateSpawn.Config.ScientistChance, betterLateSpawn.Config.FacilityGuardChance };
        }

        private float[] Chances;
        private List<string> DisconnectedPlayers = new List<string>();

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            DisconnectedPlayers.Clear();
        }

        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnVerified(Player player)
        {
            if (!Round.IsRoundStarted || DisconnectedPlayers.Contains(player.UserId) || Round.Duration.TotalSeconds > betterLateSpawn.Config.SpawnTime)
                return;

            Plugin.Coroutines.Add(Timing.CallDelayed(2f, () =>
            {
                switch (Choose())
                {
                    case 0:
                        player.SetRole(RoleTypeId.ClassD);
                        break;

                    case 1:
                        player.SetRole(RoleTypeId.Scientist);
                        break;

                    case 2:
                        player.SetRole(RoleTypeId.FacilityGuard);
                        break;
                }

                betterLateSpawn.Config.Broadcast.Show(player);
            }));
        }

        [PluginEvent(ServerEventType.PlayerLeft)]
        public void OnDestroying(Player player)
        {
            if (!global::RoundSummary.singleton._roundEnded && Round.Duration.TotalSeconds < betterLateSpawn.Config.SpawnTime)
                DisconnectedPlayers.Add(player.UserId);
        }

        private int Choose()
        {
            float total = 0;

            foreach (float elem in Chances)
            {
                total += elem;
            }

            float randomPoint = Random.value * total;

            for (int i = 0; i < Chances.Length; i++)
            {
                if (randomPoint < Chances[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= Chances[i];
                }
            }

            return Chances.Length - 1;
        }
    }
}