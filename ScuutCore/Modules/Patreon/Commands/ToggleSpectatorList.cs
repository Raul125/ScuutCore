namespace ScuutCore.Modules.Patreon.Commands;

using System;

public sealed class ToggleSpectatorList : PatreonExclusiveCommand
{
    public const string ToggleSpectatorListPermissions = "scuutcore.patreon.spectatorlist";

    protected override string Permission => ToggleSpectatorListPermissions;
    public override string Command => "spectatorlist";
    public override string[] Aliases { get; } = { };
    public override string Description => "Toggles spectatorlist instead of respawntimer.";

    protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
    {
        bool contains = PatreonExtensions.SpectatorListPlayers.Contains(sender.netId);
        if (contains)
            PatreonExtensions.SpectatorListPlayers.Remove(sender.netId);
        else
            PatreonExtensions.SpectatorListPlayers.Add(sender.netId);
        response = $"Spectatorlist is now {(contains ? "disabled" : "enabled")} for you.";
        return true;
    }
}