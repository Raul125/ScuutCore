namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    public sealed class FlyingRagdollKills : PatreonExclusiveCommand
    {
        public const string RagdollPermissions = "scuutcore.patreon.flyingragdollkilled";

        protected override string Permission => RagdollPermissions;
        public override string Command => "flyKills";

        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Toggles whether to make your victims' ragdolls fly.";
        protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
        {
            bool value = data.Prefs.FlyingRagdollKills = !data.Prefs.FlyingRagdollKills;
            response = $"Players you killed will {(value ? "now" : "no longer")} fly.";
            return true;
        }
    }
}