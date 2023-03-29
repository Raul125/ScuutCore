namespace ScuutCore.Modules.NukeRadiation
{
    using API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float RadiationTime { get; set; } = 15f;
        public float RadiationDamage { get; set; } = 2f;
        public float RadiationInterval { get; set; } = 1f;
        public string RadiationMessage { get; set; } = "You are being irradiated, leave!";
        public string EnterRadiationMessage { get; set; } = "You have entered a radioactive zone!";
        public bool EnableTimeLeftCountdown { get; set; } = true;
        public string TimeLeftCountdownMessage { get; set; } = "You have %time% seconds left!";
    }
}