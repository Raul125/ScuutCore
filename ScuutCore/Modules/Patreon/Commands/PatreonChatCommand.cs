namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using CommandSystem;
    using ScuutCore.API.Helpers;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class PatreonChatCommand : PatreonExclusiveCommand
    {
        public const string PatreonChatCommandPermissions = "scuutcore.patreon.spectatorlist";

        protected override string Permission => PatreonChatCommandPermissions;
        public override string Command => "patreonchat";
        public override string[] Aliases { get; } = { "pchat" };
        public override string Description => "Write in patreonchat.";

        protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
        {
            var senderName = sender.nicknameSync.MyNick ?? "null";
            GlobalHelpers.BroadcastToPermissions("patreonchat", $"[PCHAT] ({senderName}): {string.Join(" ", arguments)}");
            response = "Sent";
            return true;
        }
    }
}