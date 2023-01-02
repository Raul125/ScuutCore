using CommandSystem;
using PluginAPI.Core;
using MEC;
using System;
using System.Linq;

namespace ScuutCore.Modules.AdminTools
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Jail : ParentCommand
    {
        public Jail() => LoadGeneratedCommands();

        public override string Command { get; } = "scjail";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Jails or unjails a user";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Usage: scjail (player id / name)";
                return false;
            }

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }

            if (EventHandlers.JailedPlayers.Any(j => j.Userid == ply.UserId))
            {
                try
                {
                    Plugin.Coroutines.Add(Timing.RunCoroutine(EventHandlers.DoUnJail(ply)));
                    response = $"Player {ply.Nickname} has been unjailed now";
                }
                catch (Exception e)
                {
                    Log.Error($"{e}");
                    response = "Command failed. Check server log.";
                    return false;
                }
            }
            else
            {
                Plugin.Coroutines.Add(Timing.RunCoroutine(EventHandlers.DoJail(ply)));
                response = $"Player {ply.Nickname} has been jailed now";
            }

            return true;
        }
    }
}
