namespace ScuutCore.Patches
{
    using HarmonyLib;
    using MapGeneration.Distributors;
    using UnityEngine;
    [HarmonyPatch(typeof(Locker), nameof(Locker.Start))]
    public static class LockerPatch
    {

        private static bool _alreadySpawned1576;

        static LockerPatch()
        {
            CharacterClassManager.OnRoundStarted += ResetChance;
        }

        public static void ResetChance()
        {
            _alreadySpawned1576 = Random.Range(0, 100) > Plugin.Singleton.Config.Scp1576SpawnChance;
        }

        public static bool Prefix(Locker __instance)
        {
            if (_alreadySpawned1576 || __instance.StructureType != StructureType.ScpPedestal)
                return true;
            foreach (var loot in __instance.Loot)
            {
                if (loot.TargetItem != ItemType.SCP500)
                    continue;
                loot.TargetItem = ItemType.SCP1576;
                _alreadySpawned1576 = true;
                break;
            }
            return true;
        }

    }
}