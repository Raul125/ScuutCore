namespace ScuutCore.Modules.AdminTools.Commands.Jail
{
    using System;
    using System.Linq;
    using CommandSystem;
    using MEC;
    using PluginAPI.Core;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Jail : ParentCommand
    {
        public Jail() => LoadGeneratedCommands();

        public override string Command { get; } = "scjail";

        public override string[] Aliases { get; } = new string[]
        {
        };

        public override string Description { get; } = "Jails or unjails a user";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Usage: scjail (player id / name)";
                return false;
            }

            Player ply = Player.Get(int.Parse(arguments.At(0)));
            if (ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }

            if (EventHandlers.JailedPlayers.Any(j => j.UserId == ply.UserId))
            {
                Plugin.Coroutines.Add(Timing.RunCoroutine(EventHandlers.DoUnJail(ply)));
                response = $"Player {ply.Nickname} has been unjailed now";
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