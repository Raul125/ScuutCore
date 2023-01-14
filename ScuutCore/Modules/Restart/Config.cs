namespace ScuutCore.Modules.Restart
{
    using PlayerRoles;
    using ScuutCore.API;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("0 to disable")]
        public int RestartAfterRounds { get; set; } = 4;
    }
}