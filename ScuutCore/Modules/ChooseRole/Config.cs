namespace ScuutCore.Modules.ChooseRole
{
    using API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string SpawnQueue { get; set; } = "4014314031441404134044434414";
        public float ChaosProb { get; set; } = 40;
        public int MinScpAmountFor079 { get; set; } = 2;
        public bool UseBroadcastsInsteadOfHints { get; set; } = false;

        public string SelectedTeam { get; set; } = "<b>Selected Team:</b>";
        public string Random { get; set; } = "<b>Random</b>";
        public string Mtf { get; set; } = "<b><color=#036ffc>MTF</color></b>";
        public string ClassD { get; set; } = "<b><color=#fc9403>Class D</color></b>";
        public string Scientists { get; set; } = "<b><color=#ebd234>Scientists</color></b>";
        public string Scp { get; set; } = "<b><color=#eb3443>Scp</color></b>";

        public string StartingSoon { get; set; } = "<color=yellow><b>The game will be starting soon</b></color>";
        public string ServerPaused { get; set; } = "<color=yellow>Server Paused...</color>";
        public string StartingRound { get; set; } = "<color=green>Starting Round!</color>";
        public string SecondsLeft { get; set; } = "%seconds <color=red>Seconds Left</color>";
        public string PlayerHasConnected { get; set; } = "<i>1 player has connected</i>";
        public string PlayersHaveConnected { get; set; } = "<i>%players% players have connected</i>";

        public string DiscordInv { get; set; } = "<b>discord.gg/helloworld</b>";
        public string UpperText { get; set; } = "<b><color=yellow>Welcome to The Scuut!</color></b>";
        public string BottomText { get; set; } = "<b><color=yellow>Go stand next to the team you want to play as!</color></b>";
    }
}