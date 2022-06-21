using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;
using Exiled.API.Features;
using System.Collections.Generic;
using PlayerStatsSystem;

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
            ev.Player.DisableEffect(EffectType.SinkHole);

            ev.Player.ReferenceHub.scp106PlayerScript.GrabbedPosition = ev.Player.Position;
            ev.Player.EnableEffect(EffectType.Corroding);

            Plugin.Coroutines.Add(Timing.RunCoroutine(PortalAnimation(ev.Player)));

            ev.Player.Hurt(betterSinkholes.Config.EntranceDamage, DamageType.Scp106);
            ev.Player.Broadcast(6, betterSinkholes.Config.TeleportMessage);
        }

        public IEnumerator<float> PortalAnimation(Player player)
        {
            Scp106PlayerScript scp106PlayerScript = player.ReferenceHub.scp106PlayerScript;

            if (scp106PlayerScript.goingViaThePortal)
                yield break;

            scp106PlayerScript.goingViaThePortal = true;

            bool inGodMode = player.IsGodModeEnabled;
            player.IsGodModeEnabled = true;
            player.CanSendInputs = false;

            player.ReferenceHub.scp106PlayerScript.GrabbedPosition = player.Position + (Vector3.up * 1.5f);
            Vector3 startPosition = player.Position, endPosition = player.Position -= Vector3.up * 2;
            for (int i = 0; i < betterSinkholes.Config.Ticks; i++)
            {
                player.Position = Vector3.Lerp(startPosition, endPosition, i / 30f);
                yield return 0f;
            }

            player.Position = Vector3.down * 1997f;
            player.IsGodModeEnabled = inGodMode;
            player.CanSendInputs = true;

            if (Warhead.IsDetonated)
            {
                player.Kill(DeathTranslations.PocketDecay.LogLabel);
                yield break;
            }

            yield return Timing.WaitForSeconds(0.5f);
            scp106PlayerScript.goingViaThePortal = false;
        }
    }
}