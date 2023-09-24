namespace ScuutCore.Modules.ErrorLogs;

public class Message
{
    public string username { get; set; } = $"{PluginAPI.Core.Server.Port} | Error Logs";
    public string avatar_url { get; set; } = "https://w7.pngwing.com/pngs/558/606/png-transparent-error-icon-thumbnail.png";
    public string content { get; set; }

    public Message(string _content)
    {
        content = _content;
    }
}