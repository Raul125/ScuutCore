namespace ScuutCore.Patches.ScpSpeech;

using HarmonyLib;
using Modules.ScpSpeech;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using PlayerStatsSystem;
using PluginAPI.Core;

[HarmonyPatch(typeof(FpcNoclipToggleMessage), nameof(FpcNoclipToggleMessage.ProcessMessage))]
public class NoclipPatch
{
    private static bool Prefix(Mirror.NetworkConnection sender)
    {
        if (ScpSpeechModule.Instance == null)
            return true;

        if (!ReferenceHub.TryGetHubNetID(sender.identity.netId, out ReferenceHub referenceHub))
            return false;

        if (Player.TryGet(referenceHub, out Player ply) && ply.IsSCP)
        {
            if (!ScpSpeechModule.Instance.Config.PermittedRoles.Contains(ply.Role))
                return false;

            if (EventHandlers.ScpsToggled.Contains(referenceHub))
            {
                ply.ReceiveHint("\n\n\nProximity chat <mark=#ff000055>disabled</mark>");
                EventHandlers.ScpsToggled.Remove(referenceHub);
            }
            else
            {
                ply.ReceiveHint("\n\n\nProximity chat <mark=#00ff0055>enabled</mark>");
                EventHandlers.ScpsToggled.Add(referenceHub);
            }
        }

        if (!FpcNoclip.IsPermitted(referenceHub))
            return false;

        if (referenceHub.roleManager.CurrentRole is IFpcRole)
        {
            referenceHub.playerStats.GetModule<AdminFlagsStat>().InvertFlag(AdminFlags.Noclip);
            return false;
        }

        referenceHub.gameConsoleTransmission.SendToClient("Noclip is not supported for this class.", "yellow");
        return false;
    }
}