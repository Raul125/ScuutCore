namespace ScuutCore.Modules.Patreon.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using Types;
public sealed class SetCustomColor : PatreonExclusiveCommand
{
    public const string ColorPermissions = "scuutcore.patreon.customcolor";

    #region Color Dictionary

    private static readonly Dictionary<string, string> Colors = new Dictionary<string, string>
    {
        {
            "pink", "#FF96DE"
        },
        {
            "red", "#C50000"
        },
        {
            "white", "#FFFFFF"
        },
        {
            "brown", "#944710"
        },
        {
            "silver", "#A0A0A0"
        },
        {
            "light_green", "#32CD32"
        },
        {
            "crimson", "#DC143C"
        },
        {
            "cyan", "#00B7EB"
        },
        {
            "aqua", "#00FFFF"
        },
        {
            "deep_pink", "#FF1493"
        },
        {
            "tomato", "#FF6448"
        },
        {
            "yellow", "#FAFF86"
        },
        {
            "magenta", "#FF0090"
        },
        {
            "blue_green", "#4DFFB8"
        },
        {
            "orange", "#FF9966"
        },
        {
            "lime", "#BFFF00"
        },
        {
            "green", "#228B22"
        },
        {
            "emerald", "#50C878"
        },
        {
            "carmine", "#960018"
        },
        {
            "nickel", "#727472"
        },
        {
            "mint", "#98FB98"
        },
        {
            "army_green", "#4B5320"
        },
        {
            "pumpkin", "#EE7600"
        }
    };

    #endregion

    protected override string Permission => ColorPermissions;
    public override string Command => "customColor";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description => "Sets your custom badge color.";

    private const string rainbowString =
        "<color=red>r</color><color=orange>a</color><color=yellow>i</color><color=green>n</color><color=blue>b</color><color=purple>o</color><color=red>w</color>";

    protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
    {
        if (arguments.Count < 1)
        {
            response = "Usage: patreon customColor <color>";
            return false;
        }

        string color = string.Join(" ", arguments).Trim();
        string[] rainbowColors;
        rainbowColors = (data.Prefs.BadgeIndex >= data.Rank.BadgeOptions.Count
            || data.Prefs.BadgeIndex < 0
            ) ? data.Rank.BadgeOptions.First().RainbowColors : data.Rank.BadgeOptions[data.Prefs.BadgeIndex].RainbowColors;
        bool hasRainbow = rainbowColors is { Length: > 0 };

        if (hasRainbow && color == "rainbow")
        {
            data.Prefs.RainbowColors = rainbowColors;
            if (data.Prefs.BadgeIndex == Badge.Custom)
                data.UpdateBadge();
            response = $"Your badge color has been set to {rainbowString}. Use the \"patreon selectBadge custom\" command to select it.";
            return true;
        }

        if (!TryGetColor(color, out string code))
        {
            response = "Invalid color. Available ones: " + string.Join(" ", Colors.Select(e => $"<color={e.Value}>{e.Key}</color>"));
            if (hasRainbow)
                response += " " + rainbowString;
            return false;
        }

        data.Prefs.CustomBadgeColor = color;
        if (data.Prefs.BadgeIndex == Badge.Custom)
            data.Hub.serverRoles.Network_myColor = color;
        response = $"Your badge color has been set to <color={code}>{color}</color>. Use the \"patreon selectBadge custom\" command to select it.";
        return true;
    }

    public static bool TryGetColor(string color, out string code) => Colors.TryGetValue(color, out code);
}