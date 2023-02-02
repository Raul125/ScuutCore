namespace ScuutCore.Commands
{
    using System;
    using CommandSystem;
    using Footprinting;
    using NWAPIPermissionSystem;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using ScuutCore.API.Helpers;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpecialKill : ICommand
    {
        public string Command { get; } = "specialkill";
        public string[] Aliases { get; } = new string[] { "sk", "skill" };
        public string Description { get; } = "Kills a player with a special effect.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scuutcore.specialkill"))
            {
                response = "Missing perm: scuutcore.specialkill!";
                return false;
            }
            
            if (!Round.IsRoundStarted)
            {
                response = "You gotta wait for the round to start!";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage: specialkill <player> <effect>";
                return false;
            }

            Player player = Player.Get(arguments.At(0));
            if (player is null && int.TryParse(arguments.At(0), out var plyId))
                player = Player.Get(plyId);

            if (player is null)
                player = Player.GetByName(arguments.At(0));

            if (player == null)
            {
                response = "Cannot find player: " + arguments.At(0);
                return false;
            }

            switch (arguments.At(1).ToLower())
            {
                case "explode":
                case "grenade":
                case "explosion":
                case "explosive":
                    PlayerDeathEffects.PlayExplosionEffect(player);
                    player.Kill("Ate taco bell");
                    break;
                case "vaporize":
                case "dust":
                case "particle":
                    player.ReferenceHub.playerStats.KillPlayer(new DisruptorDamageHandler(new Footprint(player.ReferenceHub), -1));
                    break;
                default:
                    response = "Invalid effect: " + arguments.At(1).ToLower();
                    return false;
            }
            
            response = "Killed " + player.Nickname + " with " + arguments.At(1).ToLower() + " effect!";
            return true;
        }
    }
}