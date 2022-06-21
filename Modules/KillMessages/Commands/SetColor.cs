using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace ScuutCore.Modules.KillMessages
{
    public class SetColor : ICommand
    {
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("kmsg.color") || !sender.CheckPermission("kmsg"))
            {
                response = KillMessages.Singleton.Config.NoPerms;
                return false;
            }
            
            Player p = Player.Get(sender);
            if (arguments.Count < 1)
            {
                response = KillMessages.Singleton.Config.ColorEmpty;
                return false;
            }
            string c = arguments.ElementAt(0);
            if (string.IsNullOrEmpty(c))
            {
                response = KillMessages.Singleton.Config.ColorEmpty;
                return false;
            }
            if (!c.StartsWith("#") && !KillMessages.Singleton.Config.AvailableColors.Contains(c, StringComparison.OrdinalIgnoreCase))
            {
                response = KillMessages.Singleton.Config.ColorNotFound.Replace("$color", arguments.ElementAt(0));
                return false;
            }
            p.UpdateColor(c);

            response = KillMessages.Singleton.Config.ColorCmd;
            return true;
        }

        public string Command { get; } = "color";
        public string[] Aliases { get; } = {"setcolorkmsg", "setcolor"};
        public string Description { get; } = "Sets your kill message color";
    }
}