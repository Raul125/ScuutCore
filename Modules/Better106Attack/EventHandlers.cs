using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using PlayerStatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace ScuutCore.Modules.Better106Attack
{
    public class EventHandlers
    {
        private Better106Attack better106Attack;
        public EventHandlers(Better106Attack bta)
        {
            better106Attack = bta;
        }

        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            ev.IsAllowed = false;
            Plugin.Coroutines.Add(Timing.RunCoroutine(PortalAnimation(ev.Scp106.ReferenceHub.scp106PlayerScript, ev.Player)));
        }

        public IEnumerator<float> PortalAnimation(Scp106PlayerScript __instance, Player player)
        {
            Scp106PlayerScript victim106Script = player.ReferenceHub.scp106PlayerScript;

            if (victim106Script.goingViaThePortal)
                yield break;

            __instance.TargetHitMarker(__instance.connectionToClient, __instance.captureCooldown);
            __instance._currentServerCooldown = __instance.captureCooldown;

            victim106Script.GrabbedPosition = player.Position;
            victim106Script.goingViaThePortal = true;

            bool inGodMode = player.IsGodModeEnabled;
            player.IsGodModeEnabled = true;
            player.CanSendInputs = false;
            player.EnableEffect<Amnesia>();

            foreach (Scp079PlayerScript scp079PlayerScript in Scp079PlayerScript.instances)
            {
                scp079PlayerScript.ServerProcessKillAssist(player.ReferenceHub, ExpGainType.PocketAssist);
            }

            yield return Timing.WaitForSeconds(better106Attack.Config.Delay);

            for (uint i = 0; i < better106Attack.Config.Ticks; i++)
            {
                var pos = player.Position;
                pos.y -= i * 0.1f;
                player.Position = pos;
                yield return Timing.WaitForOneFrame;
            }

            player.Position = Vector3.down * 1993f;
            player.EnableEffect<Corroding>();
            player.IsGodModeEnabled = inGodMode;
            player.CanSendInputs = true;
            player.DisableEffect<Amnesia>();

            if (Warhead.IsDetonated)
            {
                player.Kill(DeathTranslations.Warhead.LogLabel);
            }
            else
            {
                player.Hurt(40, Exiled.API.Enums.DamageType.Scp106);
            }

            yield return Timing.WaitForSeconds(0.5f);
            victim106Script.goingViaThePortal = false;
        }
    }
}