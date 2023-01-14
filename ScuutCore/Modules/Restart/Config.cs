namespace ScuutCore.Modules.Restart
{
    using ScuutCore.API;
    using System.ComponentModel;
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("0 to disable")]
        public int RestartAfterRounds { get; set; } = 4;
    }
}