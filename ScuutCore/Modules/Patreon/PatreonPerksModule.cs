namespace ScuutCore.Modules.Patreon;

using API.Features;
using Commands;
using PluginAPI.Core;
public sealed class PatreonPerksModule : EventControllerModule<PatreonPerksModule, Config, EventHandlers>
{
    public override string Name => "Patreon Perks";

    public override void OnEnabled()
    {
        base.OnEnabled();

        foreach (var rank in Config.RankList)
        {
            foreach (var badge in rank.BadgeOptions)
            {
                if (badge.RainbowColors == null)
                    continue;
                foreach (string color in badge.RainbowColors)
                {
                    if (!SetCustomColor.TryGetColor(color, out _))
                        Log.Warning($"Invalid color \"{color}\" in badge \"{badge.Content}\" for rank \"{rank.Id}\"");
                }
            }
        }
    }
}