namespace ScuutCore.Patches
{
    using HarmonyLib;
	using PluginAPI.Core;

	[HarmonyPatch(typeof(Player), nameof(Player.ReceiveHint))]
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