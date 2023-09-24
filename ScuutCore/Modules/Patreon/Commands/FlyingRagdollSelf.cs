namespace ScuutCore.Modules.Patreon.Commands;

using System;
public sealed class FlyingRagdollSelf : PatreonExclusiveCommand
{
    public const string RagdollPermissions = "scuutcore.patreon.flyingragdollself";

    protected override string Permission => RagdollPermissions;
    public override string Command => "flyingRagdollSelf";

    public override string[] Aliases { get; } =
    {
    };

    public override string Description => "Toggles whether to make your ragdolls fly when you die.";
    protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
    {
        bool value = data.Prefs.FlyingRagdollSelf = !data.Prefs.FlyingRagdollSelf;
        response = $"You will {(value ? "now" : "no longer")} fly upon death.";
        return true;
    }
}