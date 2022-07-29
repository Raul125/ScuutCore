using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using UnityEngine;

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

        public void OnWaitingForPlayers()
        {
            Server.LaterJoinEnabled = false;
            DisconnectedPlayers.Clear();
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if (!Round.IsStarted || DisconnectedPlayers.Contains(ev.Player.UserId) || Round.ElapsedTime.TotalSeconds > betterLateSpawn.Config.SpawnTime)
                return;

            Plugin.Coroutines.Add(Timing.CallDelayed(2f, () =>
            {
                switch (Choose())
                {
                    case 0:
                        ev.Player.Role.Type = RoleType.ClassD;
                        break;

                    case 1:
                        ev.Player.Role.Type = RoleType.Scientist;
                        break;

                    case 2:
                        ev.Player.Role.Type = RoleType.FacilityGuard;
                        break;
                }

                ev.Player.Broadcast(betterLateSpawn.Config.Broadcast);
            }));
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (!Round.IsEnded && Round.ElapsedTime.TotalSeconds < betterLateSpawn.Config.SpawnTime)
            {
                DisconnectedPlayers.Add(ev.Player.UserId);
            }
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