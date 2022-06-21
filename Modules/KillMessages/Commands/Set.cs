using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using NorthwoodLib.Pools;

namespace ScuutCore.Modules.KillMessages
{
    public class Set : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("kmsg"))
            {
                response = KillMessages.Singleton.Config.NoPerms;
                return false;
            }
            
            Player p = Player.Get(sender);
            
            string msg = "";
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            foreach (string argument in arguments)
            {
                stringBuilder.Append(argument);
                if (KillMessages.Singleton.Config.BlacklistedWords.Contains(argument))
                {
                    StringBuilderPool.Shared.Return(stringBuilder);
                    response = KillMessages.Singleton.Config.BlacklistedWordsMessage;
                    return false;
                }
                stringBuilder.Append(" ");
            }
            msg = stringBuilder.ToString();
            StringBuilderPool.Shared.Return(stringBuilder);
            if (string.IsNullOrEmpty(msg))
            {
                response = KillMessages.Singleton.Config.EmptyMessage;
                return false;
            }
            if (msg.Length > KillMessages.Singleton.Config.CharLimit)
            {
                response = KillMessages.Singleton.Config.MaxChars.Replace("$limit", KillMessages.Singleton.Config.CharLimit.ToString());
                return false;
            }
            
            p.UpdateMessage(msg);
            response = KillMessages.Singleton.Config.SetCmd;
            return true;
        }

        public string Command { get; } = "setkmsg";
        public string[] Aliases { get; } = new[] {"set"};
        public string Description { get; } = "Sets your kill message";
    }
}