namespace ScuutCore.Modules.Patreon
{
    using System.Collections.Generic;
    using API.Interfaces;
    public sealed class Config : IModuleConfig
    {

        public bool IsEnabled { get; set; } = true;

        public List<string> BlacklistedBadgePhrases { get; set; } = new List<string>();
    }
}