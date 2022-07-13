using Exiled.API.Features;
using HarmonyLib;

namespace ScuutCore.Patches
{
	[HarmonyPatch(typeof(Player), nameof(Player.ShowHint))]
	public static class OverrideHintPatch
	{
		public static bool Prefix(Player __instance, string message, float duration = 3)
		{
			if (Modules.RoundSummary.EventHandlers.PreventHints) 
				return false;

			return true;
		}
	}
}