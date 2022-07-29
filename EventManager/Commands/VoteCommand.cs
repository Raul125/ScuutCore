using System;
using CommandSystem;
using EventManager.Api;
using Exiled.API.Features;

namespace EventManager.Commands
{
    public class VoteCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (EventHandlers.Status != EventStatus.Online)
            {
                response = "You can't vote now";
                return false;
            }

            var ply = Player.Get(sender);

            if (ply is null)
            {
                response = "Player is null";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Usage: .donev vote EventName";
                return false;
            }

            if (!CustomSpawner.LobbyManager.AddVote(ply, string.Join(" ", arguments)))
            {
                response = "That event does not exist / Already voted";
                return false;
            }

            response = "Voted!";
            return true;
        }

        public string Command { get; } = "Vote";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "Vote command for the EventManager plugin";
    }
}