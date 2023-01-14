namespace ScuutCore.Modules.KillMessages
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public string Message { get; set; } = "Killed <color=red>{name}</color>";
    }
}