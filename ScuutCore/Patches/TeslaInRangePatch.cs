namespace ScuutCore.Patches
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using HarmonyLib;
    using Modules.Teslas;

    [HarmonyPatch]
    public static class TeslaInRangePatch
    {

        public static IEnumerable<MethodInfo> TargetMethods()
        {
            yield return AccessTools.Method(typeof(TeslaGate), nameof(TeslaGate.PlayerInRange));
            yield return AccessTools.Method(typeof(TeslaGate), nameof(TeslaGate.PlayerInIdleRange));
        }

        public static bool DisableTriggering(ReferenceHub hub) => Teslas.Singleton.Config.Roles.Contains(hub.roleManager.CurrentRole.RoleTypeId);

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var list = new List<CodeInstruction>(instructions);
            var label = generator.DefineLabel();
            list[0].labels.Add(label);
            list.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                CodeInstruction.Call(typeof(TeslaInRangePatch), nameof(DisableTriggering)),
                new CodeInstruction(OpCodes.Brfalse, label),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ret)
            });

            return list;
        }
    }
}