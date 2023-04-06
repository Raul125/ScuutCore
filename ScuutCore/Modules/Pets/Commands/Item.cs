using CommandSystem;
using PluginAPI.Core;
using ScuutCore.API.Features;
using System;
using NWAPIPermissionSystem;
using ScuutCore.Modules.Patreon;

namespace ScuutCore.Modules.Pets
{
    public class Item : ICommand
    {
        public string Command { get; set; } = "item";

        public string[] Aliases { get; set; } = { "i" };

        public string Description { get; set; } = "Reset your pet data.";

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
            if (!Enum.TryParse(arguments.At(0), true, out ItemType type))
            {
                response = "Invalid ItemType";
                return false;
            }

            data.Prefs.PetData.ItemType = type;
            scuutPlayer.Pet.RefreshData(data.Prefs.PetData);
            response = "Done!";
            return true;
        }
    }
}
