namespace ScuutCore.Modules.PocketFall
{
    using ScuutCore.API.Features;
    using PluginAPI.Core;
    using UnityEngine;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PlayerStatsSystem;
    using MEC;
    using System.Collections.Generic;

    public sealed class EventHandlers : InstanceBasedEventHandler<PocketFall>
    {
        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool OnPlayerDamage(ScuutPlayer target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (target.EnteringPocket && damageHandler is UniversalDamageHandler dmg 
                && dmg.TranslationId == DeathTranslations.Falldown.Id)
                return false;

            return true;
        }

        [PluginEvent(ServerEventType.PlayerEnterPocketDimension)]
        public bool OnPlayerEnterPocketDimension(ScuutPlayer player)
        {
            if (!player.EnteringPocket)
            {
                player.EnteringPocket = true;
                Plugin.Coroutines.Add(Timing.RunCoroutine(goPocket(player)));
            }

            return false;
        }

        private IEnumerator<float> goPocket(ScuutPlayer player)
        {
            yield return Timing.WaitForSeconds(Module.Config.Delay);

            for (int i = 0; i < Module.Config.Ticks; i++)
            {
                player.Position -= Vector3.down;
                yield return Timing.WaitForOneFrame;
            }

            player.SendToPocketDimension();

            yield return Timing.WaitForSeconds(0.1f);
            player.EnteringPocket = false;
        }
    }
}