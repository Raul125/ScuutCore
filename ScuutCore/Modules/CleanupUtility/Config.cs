namespace ScuutCore.Modules.CleanupUtility
{
    using System.Collections.Generic;
    using API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool ClearRagdolls { get; set; } = true;
        public bool ClearItems { get; set; } = true;
        public Dictionary<ItemType, float> CleanDelay { get; set; } = new Dictionary<ItemType, float>()
        {
            [ItemType.KeycardJanitor] = -1f,
        };
        public float DefaultDelay { get; set; } = 300f;
        public bool UseFastZoneCheck { get; set; } = true;
    }
}