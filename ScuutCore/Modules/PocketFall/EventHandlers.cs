namespace ScuutCore.Modules.PocketFall
{
    using System.Collections.Generic;
    using API.Features;
    using CustomPlayerEffects;
    using MEC;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<PocketFall>
    {
        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool OnPlayerDamage(ScuutPlayer target, Player attacker, DamageHandlerBase damageHandler)
        {
            return !target.EnteringPocket || damageHandler is not UniversalDamageHandler dmg
                                          || dmg.TranslationId != DeathTranslations.Falldown.Id;

        }

        [PluginEvent(ServerEventType.PlayerEnterPocketDimension)]
        public bool OnPlayerEnterPocketDimension(ScuutPlayer player)
        {
            if (!player.EnteringPocket)
            {
                player.EnteringPocket = true;
                Plugin.Coroutines.Add(Timing.RunCoroutine(SendToPocket(player)));
            }

            return false;
        }

        private IEnumerator<float> SendToPocket(ScuutPlayer player)
        {
            yield return Timing.WaitForSeconds(Module.Config.Delay);

            for (int i = 0; i < Module.Config.Ticks; i++)
            {
                player.Position += Vector3.down;
                yield return Timing.WaitForOneFrame;
            }

            if (Warhead.IsDetonated)
                player.Kill();
            else
            {
                player.SendToPocketDimension();
                player.EffectsManager.EnableEffect<Corroding>();
            }

            yield return Timing.WaitForSeconds(0.1f);
            player.EnteringPocket = false;
        }
    }
}