using System;
using CommandSystem;
using EventManager.Api;
using Exiled.API.Features;

namespace EventManager.Commands
{
    public class UseCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(sender);
            
            if (ply is null)
            {
                response = "This command can only be used by a player connected to the server";
                return false;
            }

            if (Player.Dictionary.Count < MainClass.Instance.Config.MinPlayers)
            {
                response = "There are not enough users to do that.";
                return false;
            }

            if (string.IsNullOrEmpty(ply.GroupName) || !MainClass.Instance.Config.DonationGroups.ContainsKey(ply.GroupName))
            {
                response = "Group not in the config list.";
                return false;
            }

            if (Database.IsInCooldown(ply))
            {
                response = "You have to wait to use this command again.\nYou can check your cooldown with .event cooldown";
                return false;
            }

            if (EventHandlers.Status != EventStatus.Offline)
            {
                response = "Someone already called an event next round.";
                return false;
            }
            
            if(!Database.Cache.ContainsKey(ply.UserId))
                Database.Cache.Add(ply.UserId, 0);

            Database.Cache[ply.UserId] = DateTime.UtcNow.Ticks;
            Database.Save();
            
            EventHandlers.Status = EventStatus.NextRound;

            response = "Calling an event next round.";
            return true;
        }

        public string Command { get; } = "Use";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "Use command for the EventManager plugin";
    }
}