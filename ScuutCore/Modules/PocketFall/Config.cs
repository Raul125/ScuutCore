namespace ScuutCore.Modules.PocketFall
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float Delay { get; set; } = 1f;
        public int Ticks { get; set; } = 25;
    }
}