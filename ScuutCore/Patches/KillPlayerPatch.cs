namespace ScuutCore.Patches;

using HarmonyLib;
using Modules.Patreon;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using UnityEngine;

[HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.KillPlayer))]
internal static class KillPlayerPatch
{

    private static bool Prefix(PlayerStats __instance, DamageHandlerBase handler)
    {
        if (handler is UniversalDamageHandler udh && udh.TranslationId == DeathTranslations.PocketDecay.Id)
            return true;
        float multiplier = PatreonExtensions.GetRagdollVelocityMultiplier(handler, __instance._hub);
        if (multiplier <= 1f)
            return true;
        switch (handler)
        {
            case FirearmDamageHandler firearm:
            {
                if (!FirearmDamageHandler.HitboxToForce.TryGetValue(firearm.Hitbox, out float hitbox))
                    return true;
                float hitboxMultiplier = hitbox * (FirearmDamageHandler.AmmoToForce.TryGetValue(firearm._ammoType, out float ammo) ? ammo : 1f);
                var force = new Vector3(firearm._hitDirectionX, 0.0f, firearm._hitDirectionZ) * hitboxMultiplier;
                firearm.StartVelocity = force * multiplier;
                break;
            }
            case Scp939DamageHandler scp939:
            {
                var velocity = (__instance._hub.transform.position - scp939.Attacker.Hub.transform.position).normalized * 3;
                velocity.y = 0;
                scp939.StartVelocity = velocity * multiplier;
                break;
            }
            case StandardDamageHandler standard:
                standard.StartVelocity *= multiplier;
                break;
        }

        return true;
    }

}