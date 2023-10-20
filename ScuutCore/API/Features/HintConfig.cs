namespace ScuutCore.API.Features;

using PluginAPI.Core;

public class HintConfig
{
    public HintConfig(string ms)
    {
        Message = ms;
    }

#pragma warning disable CS8618
    public HintConfig()
#pragma warning restore CS8618
    {
    }

    public string Message { get; set; }
    public int Time { get; set; } = 5;

    public void Show(Player ply)
    {
        ply.ReceiveHint(Message, Time);
    }

    public void ShowAll()
    {
        foreach (var player in Player.GetPlayers())
        {
            if (!player.IsServer)
                player.ReceiveHint(Message, Time);
        }
    }
}