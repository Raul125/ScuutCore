using CommandSystem;
using PluginAPI.Core;
using ScuutCore.API.Features;
using System;

namespace ScuutCore.Modules.Pets
{
    public class Hide : ICommand
    {
        public string Command { get; set; } = "hide";

        public string[] Aliases { get; set; } = { "h" };

        public string Description { get; set; } = "Hide your pet.";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender);
            ScuutPlayer scuutPlayer = ScuutPlayer.Get(ply.ReferenceHub);

            if (scuutPlayer.Pet is null)
            {
                response = "You don't have a pet.";
                return false;
            }

            scuutPlayer.Pet.Destroy();
            scuutPlayer.Pet = null;
            response = "Done!";
            return true;
        }
    }
}
