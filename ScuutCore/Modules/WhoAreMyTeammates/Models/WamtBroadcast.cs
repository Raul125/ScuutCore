using PlayerRoles;

namespace ScuutCore.Modules.WhoAreMyTeammates
{
    public class WamtBroadcast
    {
        public bool IsEnabled { get; set; }

        public Team Team { get; set; }

        public string Contents { get; set; }

        public string AloneContents { get; set; }

        public int Delay { get; set; }

        public int MaxPlayers { get; set; }

        public DisplayType Type { get; set; }

        public ushort Time { get; set; }
    }
}