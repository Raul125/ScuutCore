namespace ScuutCore.Modules.AntiAFK;

using System.ComponentModel;
using API.Features;
using API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;

    [Description("The minimum amount of players that should be on the server to run the afk check.")]
    public int MinimumPlayers { get; set; } = 2;

    [Description("Whether tutorials will be ignored from afk checks.")]
    public bool IgnoreTutorials { get; set; } = true;

    [Description("The amount of time, in seconds, that a player will receive a warning that they have hit the maximum afk time.")]
    public int GraceTime { get; set; } = 15;

    [Description("The amount of time, in seconds, before a player will be detected as afk.")]
    public int AfkTime { get; set; } = 45;

    [Description("Whether the player should be replaced by another spectator.")]
    public bool TryReplace { get; set; } = true;

    [Description("The amount of times a player will be forced to spectate before they are kicked.")]
    public int SpectateLimit { get; set; } = 2;

    // Translations

    [Description("The broadcast to send when a player is in the grace period for being afk.")]
    public string GracePeriodWarning { get; set; } = "<color=red>You will be classified as AFK in</color> {0} <color=red>$seconds!</color>";

    [Description("The reason to display when a player is kicked for being afk.")]
    public string KickReason { get; set; } = "[Kicked by uAFK]\nYou were AFK for too long!";

    [Description("The broadcast to send when a player is forced to spectator for being afk.")]
    public BroadcastConfig SpectatorForced { get; set; } = new BroadcastConfig("You were detected as AFK and moved to spectator!", 30);
    public bool DisableBaseGameAfk { get; set; } = true;
}