using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.TpsLogger
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool EnableWebhook { get; set; } = true;

        [Description("The webhook url.")]
        public string Url { get; set; } = string.Empty;

        [Description("The interval, in seconds, to log the tps.")]
        public float Interval { get; set; } = 60f;

        [Description("Whether webhooks will be sent while the server is in idle mode.")]
        public bool LogIdle { get; set; } = false;

        [Description("The embed header.")]
        public string Header { get; set; } = "TPS Logger";

        [Description("The color of the embed.")]
        public string Color { get; set; } = "#808080";

        [Description("The literal translation for 'players'.")]
        public string Players { get; set; } = "Players";

        [Description("The literal translation for 'TPS'.")]
        public string Tps { get; set; } = "TPS";
    }
}