namespace ScuutCore.Modules.ErrorLogs
{
    using API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string Url { get; set; } = "";
    }
}