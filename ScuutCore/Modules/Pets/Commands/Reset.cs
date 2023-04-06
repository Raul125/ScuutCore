using CommandSystem;
using PluginAPI.Core;
using ScuutCore.API.Features;
using System;
using NWAPIPermissionSystem;
using ScuutCore.Modules.Patreon;

namespace ScuutCore.Modules.Pets
{
    public class Reset : ICommand
    {
        public string Command { get; set; } = "reset";

        public string[] Aliases { get; set; } = { "r" };

        public string Description { get; set; } = "Reset your pet's data.";

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

            data.Prefs.PetData = new()
            {
                Name = $"{ply.Nickname}'s Pet"
            };

            scuutPlayer.Pet.RefreshData(data.Prefs.PetData);
            response = "Done!";
            return true;
        }
    }
}
