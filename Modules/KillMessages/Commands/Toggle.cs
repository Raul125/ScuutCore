using System;
using CommandSystem;
using Exiled.API.Features;

namespace ScuutCore.Modules.KillMessages
{
    public class Toggle : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player p = Player.Get(sender);
            
            p.UpdateDisabled();

            response = p.GetDisabled() ? KillMessages.Singleton.Config.MessagesHidden : KillMessages.Singleton.Config.MessagesNotHidden;
            return true;
        }

        public string Command { get; } = "togglekmsg";
        public string[] Aliases { get; } = {"toggle"};
        public string Description { get; } = "Toggles whether or not you can see kill messages";
    }
}