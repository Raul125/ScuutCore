namespace ScuutCore.Patches
{
    using System.Collections.Generic;
    using HarmonyLib;
    using MEC;

    [HarmonyPatch(typeof(Timing), nameof(Timing.RunCoroutine), typeof(IEnumerator<float>), typeof(Segment))]
    public static class OverrideSegmentPatch
    {
        public static void Prefix(ref Segment segment) => segment = Segment.FixedUpdate;
    }
}