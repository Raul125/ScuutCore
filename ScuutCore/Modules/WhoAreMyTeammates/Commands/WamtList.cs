namespace ScuutCore.Modules.WhoAreMyTeammates.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using CommandSystem;
    using PlayerRoles;
    using PluginAPI.Core;
    using RemoteAdmin;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public sealed class WamtList : ICommand
    {
        public string Command => "WamtList";
        public string[] Aliases { get; } =
        {
            "WL",
            "SCPList",
            "ListSCPs"
        };
        public string Description => "Lists SCPs in the current round";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                if (playerSender.ReferenceHub.roleManager._curRole.Team != Team.SCPs)
                {
                    response = "You must be an SCP to run this command!";
                    return false;
                }

                var scpList = Player.GetPlayers().Where(x => x.Role.GetTeam() is Team.SCPs).ToArray();
                var scpNames = new StringBuilder();
                for (int i = 0; i < scpList.Length; i++)
                {
                    var scp = scpList[i];
                    scpNames.Append(scp.ReferenceHub.roleManager._curRole.RoleName);
                    scpNames.Append(i != scpList.Length - 1 ? ", " : ".");
                }

                var combined = scpNames.ToString();
                Player.Get(sender).SendBroadcast($"<color=red>The Following SCPs are ingame: {combined}</color>", 10);
                response = $"The Following SCPs are ingame: {combined}";
                return true;
            }
            else
            {
                var scpList = Player.GetPlayers().Where(x => x.Role.GetTeam() is Team.SCPs).ToArray();
                var scpNames = new StringBuilder();
                for (int i = 0; i < scpList.Length; i++)
                {
                    var scp = scpList[i];
                    scpNames.Append(scp.ReferenceHub.roleManager._curRole.RoleName);
                    scpNames.Append(i != scpList.Length - 1 ? ", " : ".");
                }

                response = $"The Following SCPs are ingame: {scpNames}";
                return true;
            }
        }
    }
}