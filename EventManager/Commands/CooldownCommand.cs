using System;
using CommandSystem;
using Exiled.API.Features;
using EventManager.Api;

namespace EventManager.Commands
{
    public class CooldownCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (!Database.IsInCooldown(player))
            {
                response = "You don't have cooldown, you can start an event right now.";
                return true;
            }

            // Database.Cache[player.UserId] > DateTime.UtcNow.Ticks - MainClass.Instance.Config.DonationGroups[userGroup]
            var dateTime = new DateTime(Database.Cache[player.UserId]);
            var minutesRemaining = (dateTime + TimeSpan.FromSeconds(MainClass.Instance.Config.DonationGroups[player.GroupName])) - DateTime.Now;

            response = $"You can start an event in {minutesRemaining.Hours} hours {minutesRemaining.Minutes} minutes and {minutesRemaining.Seconds} seconds";
            return true;
        }

        public string Command { get; } = "cooldown";
        public string[] Aliases { get; } = {"cd"};
        public string Description { get; } = "Checks your event cooldown";
    }
}