using System;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using NorthwoodLib.Pools;

namespace ScuutCore.Modules.KillMessages
{
    public class Delete : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("kmsg"))
            {
                response = KillMessages.Singleton.Config.NoPerms;
                return false;
            }
            
            Player.Get(sender).UpdateMessage();
            response = KillMessages.Singleton.Config.DeleteCmd;
            return true;
        }

        public string Command { get; } = "deletekmsg";
        public string[] Aliases { get; } = new[] {"delete", "del"};
        public string Description { get; } = "Deletes your kill message";
    }
}