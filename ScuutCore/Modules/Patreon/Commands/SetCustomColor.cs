namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    public sealed class SetCustomColor : PatreonExclusiveCommand
    {

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

        protected override string Permission => "scuutcore.patreon.customcolor";
        public override string Command => "customColor";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Sets your custom badge color.";
        protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Usage: patreon customColor color";
                return false;
            }

            string color = string.Join(" ", arguments).Trim();
            if (!Colors.TryGetValue(color, out string code))
            {
                response = "Invalid color. Available ones: " + string.Join(" ", Colors.Select(e => $"<color={e.Value}>{e.Key}</color>"));
                return false;
            }

            data.Prefs.CustomBadgeColor = color;
            if (data.Prefs.BadgeIndex == Badge.Custom)
                data.Hub.serverRoles.Network_myColor = color;
            response = $"Your badge color has been set to <color={code}>{color}</color>. Use the \"patreon selectBadge custom\" command to select it.";
            return true;
        }
    }
}