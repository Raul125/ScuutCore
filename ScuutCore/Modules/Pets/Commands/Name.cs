using CommandSystem;
using PluginAPI.Core;
using ScuutCore.API.Features;
using System;
using NWAPIPermissionSystem;
using ScuutCore.Modules.Patreon;

namespace ScuutCore.Modules.Pets
{
    public class Name : ICommand
    {
        public string Command { get; set; } = "name";

        public string[] Aliases { get; set; } = { "n" };

        public string Description { get; set; } = "Change your pet's name";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender);
            if (!sender.CheckPermission("scuutcore.patreon.pets"))
            {
                response = "You don't have permissions!";
                return false;
            }

            if (!ply.ReferenceHub.TryGetComponent<PatreonData>(out var data))
            {
                response = "You don't have permissions!";
                return false;
            }

            ScuutPlayer scuutPlayer = ScuutPlayer.Get(ply.ReferenceHub);

            string newName = string.Join(" ", arguments);
            data.Prefs.PetData.Name = newName;
            scuutPlayer.Pet.RefreshData(data.Prefs.PetData);
            response = "Done!";
            return true;
        }
    }
}
