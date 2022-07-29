﻿namespace ScuutCore.Modules.WhoAreMyTeammates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using Mirror;
    using NorthwoodLib.Pools;
    using RemoteAdmin;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class WamtList : ICommand
    {
        public string Command { get; } = "WamtList";
        public string[] Aliases { get; } = { "WL", "SCPList", "ListSCPs" };
        public string Description { get; } = "Lists SCPs in the current round";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                if (!playerSender.CharacterClassManager.IsAnyScp())
                {
                    response = "You must be an SCP to run this command!";
                    return false;
                }

                var scps = Player.Get(Team.SCP);
                var scpNames = new List<string>();
                foreach (var scp in scps)
                {
                    scpNames.Add(scp.ReferenceHub.characterClassManager.CurRole.fullName);                  
                    if (scp != scps.Last())
                        scpNames.Append(", ");
                    else
                        scpNames.Append(".");

                }

                string NameString = String.Join(",", scpNames);
                Player.Get(sender).Broadcast(10, $"<color=red>The Following SCPs are ingame: {NameString}</color>");
                response = $"The Following SCPs are ingame: {NameString}";
                return true;
            }
            else
            {
                var scps = Player.Get(Team.SCP);
                var scpNames = new List<string>();
                foreach (var scp in scps)
                {
                    scpNames.Add(scp.ReferenceHub.characterClassManager.CurRole.fullName);
                    if (scp != scps.Last())
                        scpNames.Append(", ");
                    else
                        scpNames.Append(".");
                }

                response = $"The Following SCPs are ingame: {scpNames}";
                return true;
            }
        }
    }
}