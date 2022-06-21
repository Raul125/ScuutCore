using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace ScuutCore.Modules.BetterLateSpawn
{
    public class EventHandlers
    {
        private BetterLateSpawn betterLateSpawn;
        public EventHandlers(BetterLateSpawn bls)
        {
            betterLateSpawn = bls;
        }

        private List<string> DisconnectedPlayers = new List<string>();

        public void OnWaitingForPlayers()
        {
            DisconnectedPlayers.Clear();
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if (!Round.IsStarted || DisconnectedPlayers.Contains(ev.Player.UserId) || Round.ElapsedTime.TotalSeconds > betterLateSpawn.Config.SpawnTime)
                return;

            Plugin.Coroutines.Add(Timing.CallDelayed(2f, () =>
            {
                switch (Plugin.Random.Next(0, 2))
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

                ev.Player.Broadcast(10, betterLateSpawn.Config.Broadcast);
            }));
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Reason == Exiled.API.Enums.SpawnReason.LateJoin)
                ev.IsAllowed = false;
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (!Round.IsEnded && Round.ElapsedTime.TotalSeconds < betterLateSpawn.Config.SpawnTime)
            {
                DisconnectedPlayers.Add(ev.Player.UserId);
            }
        }
    }
}