using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace ScuutCore.Modules.CleanupUtility
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CleanupCommand : ICommand
    {
        public string Command { get; set; } = "cleanup";

        public string[] Aliases { get; set; } = null;

        public string Description { get; set; } = "Remove ragdolls and items";

        public string InsufficientPermission { get; set; } = "You do not have permission to use this command.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {;
            if (sender.CheckPermission("ScuutCore.cleanup"))
            {
                response = InsufficientPermission;
                return false;
            }

            foreach (var ragdoll in Map.Ragdolls)
            {
                if (ragdoll != null)
                    ragdoll.Delete();
            }


            foreach (var item in Map.Pickups)
            {
                if (item != null)
                    item.Destroy();
            }

            response = "Done";
            return true;
        }
    }
}