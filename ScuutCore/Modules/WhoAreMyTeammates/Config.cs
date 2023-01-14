namespace ScuutCore.Modules.WhoAreMyTeammates
{
    using PlayerRoles;
    using ScuutCore.API;
    using System.Collections.Generic;
    using System.ComponentModel;
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("The delay after the round starts before broadcasts will be displayed.")]
        public float DelayTime { get; set; } = 0;

        [Description("Sets broadcasts for each class. Use %list% for the player names/SCP names and %count% for number of teammates")]
        public List<WamtBroadcast> WamtBroadcasts { get; set; } = new List<WamtBroadcast>
        {
            new WamtBroadcast()
            {
                Team = Team.SCPs,
                Contents = "Welcome to the<color=red><b> SCP Team.</b></color><color=aqua> The following SCPs are on this team: </color><color=red>%list%</color>",
                AloneContents = "<color=red>Attention - You are the <b>only</b> SCP This game. Good Luck.</color>",
                Delay = 20,
                MaxPlayers = -1,
                Type = DisplayType.Hint,
                Time = 10,
                IsEnabled = true,
            },
            new WamtBroadcast()
            {
                Team = Team.FoundationForces,
                Contents = "Welcome to the<color=grey><b> MTF Team.</b></color><color=aqua> The following Guards are on this team: </color><color=grey>%list%</color>",
                AloneContents = "<color=grey>Attention - You are the <b>only</b> Facility Guard this game. Good Luck.</color>",
                Delay = 3,
                MaxPlayers = -1,
                Type = DisplayType.Hint,
                Time = 10,
                IsEnabled = true,
            },
            new WamtBroadcast()
            {
                Team = Team.Scientists,
                Contents = "Welcome to the<color=yellow><b> Scientist Team.</b></color><color=aqua> These are your partners in science: </color><color=yellow>%list%</color>",
                AloneContents = "<color=yellow>Attention - You are the <b>only</b> Scientist this game. Good Luck.</color>",
                Delay = 3,
                MaxPlayers = -1,
                Type = DisplayType.Hint,
                Time = 10,
                IsEnabled = true,
            },
            new WamtBroadcast()
            {
                Team = Team.ClassD,
                Contents = "Welcome to the<color=orange><b> Class D Team.</b></color> The following class Ds are on this team: <color=orange>%list%</color>",
                AloneContents = "<color=orange>Attention - You are the <b>only</b> Class D Personnel this game. Good Luck.</color>",
                Delay = 3,
                MaxPlayers = -1,
                Type = DisplayType.Hint,
                Time = 10,
                IsEnabled = true,
            },
            new WamtBroadcast
            {
                Team = Team.ChaosInsurgency,
                Contents = "Welcome to the<color=green><b> Chaos Insurgency.</b></color> The following players are your comrades: <color=green>%list%</color>",
                AloneContents = "<color=green>Attention - You are the <b>only</b> Insurgent this game. Good Luck.</color>",
                Delay = 3,
                MaxPlayers = -1,
                Type = DisplayType.Hint,
                Time = 10,
                IsEnabled = true,
            },
        };
    }
}