using CommandSystem;
using PluginAPI.Core;
using ScuutCore.API.Features;
using System;
using NWAPIPermissionSystem;

namespace ScuutCore.Modules.Pets
{
    public class Show : ICommand
    {
        public string Command { get; set; } = "show";

        public string[] Aliases { get; set; } = { "s" };

        public string Description { get; set; } = "Show your pet.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender);
            ScuutPlayer scuutPlayer = ScuutPlayer.Get(ply.ReferenceHub);

            if (scuutPlayer.Pet is not null)
            {
                response = "You already have a pet.";
                return false;
            }

            if (!sender.CheckPermission("scuutcore.patreon.pets"))
            {
                response = "You don't have permissions!";
                return false;
            }

            scuutPlayer.Pet = Pet.Create(scuutPlayer);
            response = "Done!";
            return true;
        }
    }
}
