namespace ScuutCore.API.Features;

using PluginAPI.Core;

public class HintConfig
{
    public HintConfig(string ms)
    {
        Message = ms;
    }

    public HintConfig()
    {
    }

    public string Message { get; set; }
    public int Time { get; set; } = 5;

    public void Show(Player ply = null)
    {
        if (ply != null)
            ply.ReceiveHint(Message, Time);
        else
        {
            foreach (var player in Player.GetPlayers())
                player.ReceiveHint(Message, Time);
        }
    }
}