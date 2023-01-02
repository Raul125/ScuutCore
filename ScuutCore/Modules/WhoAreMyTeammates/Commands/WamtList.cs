﻿namespace ScuutCore.Modules.WhoAreMyTeammates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandSystem;
    using PlayerRoles;
    using PluginAPI.Core;
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
                if (playerSender.ReferenceHub.roleManager._curRole.Team != Team.SCPs)
                {
                    response = "You must be an SCP to run this command!";
                    return false;
                }

                var scps = Player.GetPlayers().Where(x => x.Role.GetTeam() is Team.SCPs);
                var scpNames = new List<string>();
                foreach (var scp in scps)
                {
                    scpNames.Add(scp.ReferenceHub.roleManager._curRole.RoleName);                  
                    if (scp != scps.Last())
                        scpNames.Append(", ");
                    else
                        scpNames.Append(".");

                }

                string NameString = String.Join(",", scpNames);
                Player.Get(sender).SendBroadcast($"<color=red>The Following SCPs are ingame: {NameString}</color>", 10);
                response = $"The Following SCPs are ingame: {NameString}";
                return true;
            }
            else
            {
                var scps = Player.GetPlayers().Where(x => x.Role.GetTeam() is Team.SCPs);
                var scpNames = new List<string>();
                foreach (var scp in scps)
                {
                    scpNames.Add(scp.ReferenceHub.roleManager._curRole.RoleName);
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