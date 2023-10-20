namespace ScuutCore.API.Features;

using PluginAPI.Core;

public class BroadcastConfig
{
    public BroadcastConfig(string ms, ushort time = 10)
    {
        Text = ms;
        Duration = time;
    }

    public BroadcastConfig()
    {
    }

    public bool AbleToShow { get; set; } = true;
    public string Text { get; set; } = "";
    public bool ClearPrevious { get; set; } = false;
    public ushort Duration { get; set; } = 10;
    public Broadcast.BroadcastFlags BroadcastFlags { get; set; } = Broadcast.BroadcastFlags.Normal;

    public void Show(Player ply)
    {
        if (!AbleToShow)
            return;

        ply.SendBroadcast(Text, Duration, BroadcastFlags, ClearPrevious);
    }

    public void ShowAll()
    {
        if (!AbleToShow)
            return;

        Map.Broadcast(Duration, Text, BroadcastFlags, ClearPrevious);
    }
}