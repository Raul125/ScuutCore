namespace ScuutCore.Patches;

using HarmonyLib;
using Modules.Patreon;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using ScuutCore.API.Extensions;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using static HarmonyLib.AccessTools;

[HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.KillPlayer))]
internal static class KillPlayerPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = instructions.BeginTranspiler();

        // KillPlayerPatch.OnKillPlayer(this, handler);
        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Ldarg_1),
            new(OpCodes.Call, Method(typeof(KillPlayerPatch), nameof(OnKillPlayer))),
        });

        return newInstructions.FinishTranspiler();
    }

    private static void OnKillPlayer(PlayerStats stats, DamageHandlerBase handler)
    {
        if (handler is UniversalDamageHandler udh && udh.TranslationId == DeathTranslations.PocketDecay.Id)
            return;

        float multiplier = PatreonExtensions.GetRagdollVelocityMultiplier(handler, stats._hub);

        if (multiplier <= 1f)
            return;

        switch (handler)
        {
            case FirearmDamageHandler firearm:
                {
                    if (!FirearmDamageHandler.HitboxToForce.TryGetValue(firearm.Hitbox, out float hitbox))
                        return;

                    float hitboxMultiplier = hitbox * (FirearmDamageHandler.AmmoToForce.TryGetValue(firearm._ammoType, out float ammo) ? ammo : 1f);
                    var force = new Vector3(firearm._hitDirectionX, 0.0f, firearm._hitDirectionZ) * hitboxMultiplier;
                    firearm.StartVelocity = force * multiplier;
                        return;
                }

            case Scp939DamageHandler scp939:
                {
                    var velocity = (stats._hub.transform.position - scp939.Attacker.Hub.transform.position).normalized * 3;
                    velocity.y = 0;
                    scp939.StartVelocity = velocity * multiplier;
                    return;
                }

            case StandardDamageHandler standard:
                standard.StartVelocity *= multiplier;
                return;
        }
    }
}