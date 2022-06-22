using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;
using Exiled.API.Features;
using System.Collections.Generic;
using PlayerStatsSystem;
using CustomPlayerEffects;

namespace ScuutCore.Modules.BetterSinkholes
{
    public class EventHandlers
    {
        private BetterSinkholes betterSinkholes;
        public EventHandlers(BetterSinkholes btc)
        {
            betterSinkholes = btc;
        }

        public void OnWalkingOnSinkhole(WalkingOnSinkholeEventArgs ev)
        {
            if (ev.Player.SessionVariables.ContainsKey("IsNPC"))
                return;

            if (ev.Player.IsScp && ev.Sinkhole.SCPImmune)
                return;

            if ((ev.Player.Position - ev.Sinkhole.transform.position).sqrMagnitude > betterSinkholes.Config.TeleportDistance * betterSinkholes.Config.TeleportDistance)
                return;

            ev.IsAllowed = false;

            ev.Player.ReferenceHub.scp106PlayerScript.GrabbedPosition = ev.Player.Position;

            Plugin.Coroutines.Add(Timing.RunCoroutine(PortalAnimation(ev.Player)));
        }

        private IEnumerator<float> PortalAnimation(Player player)
        {
            if (player.ReferenceHub.scp106PlayerScript.goingViaThePortal)
                yield break;

            player.ReferenceHub.scp106PlayerScript.goingViaThePortal = true;

            var pos = player.Position;
            for (uint i = 0; i < betterSinkholes.Config.Ticks; i++)
            {
                pos.y -= i * betterSinkholes.Config.PositionChange;
                player.Position = pos;
                yield return Timing.WaitForOneFrame;
            }

            player.Position = Vector3.down * 1993f;
            player.EnableEffect<Corroding>();
            player.DisableEffect<SinkHole>();
            player.Broadcast(betterSinkholes.Config.TeleportMessage);
            player.Hurt(betterSinkholes.Config.EntranceDamage, DamageType.Scp106);

            player.ReferenceHub.scp106PlayerScript.goingViaThePortal = false;
        }
    }
}