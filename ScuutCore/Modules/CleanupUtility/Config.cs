namespace ScuutCore.Modules.CleanupUtility
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        // public float CleanDelay { get; set; } = 300f;
        // public bool DestroyRagdolls { get; set; } = true;
        public bool ClearItems { get; set; } = true;
        public bool UseFastZoneCheck { get; set; } = true;
    }
}