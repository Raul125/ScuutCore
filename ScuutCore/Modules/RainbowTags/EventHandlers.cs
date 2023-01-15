using PluginAPI.Core;
using UnityEngine;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using MEC;
using System.Linq;

namespace ScuutCore.Modules.RainbowTags
{
    using ScuutCore.Modules.RainbowTags.Component;

    public class EventHandlers
    {
        private RainbowTags rainbowTags;
        public EventHandlers(RainbowTags rb)
        {
            rainbowTags = rb;
        }

        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnJoined(Player player)
        {
            if (player is null || player.GameObject is null)
                return;

            Timing.CallDelayed(2f, () =>
            {
                bool hasColors = TryGetColors(GetKey(player.ReferenceHub.serverRoles.Group), out string[] colors);
                if (player.ReferenceHub.serverRoles.Group != null && hasColors)
                {
                    RainbowTagController controller = player.GameObject.AddComponent<RainbowTagController>();
                    controller.Colors = colors;
                    controller.Interval = rainbowTags.Config.TagInterval;
                    return;
                }

                if (!player.GameObject.TryGetComponent(out RainbowTagController rainbowTagController))
                    return;

                if (hasColors)
                    rainbowTagController.Colors = colors;
                else
                    Object.Destroy(rainbowTagController);
            });
        }

        private bool TryGetColors(string rank, out string[] availableColors)
        {
            availableColors = null;
            return !string.IsNullOrEmpty(rank) && rainbowTags.Config.Sequences.TryGetValue(rank, out availableColors);
        }

        public string GetKey(UserGroup @this) => ServerStatic.PermissionsHandler._groups
            .FirstOrDefault(pair => pair.Value.Equals(@this)).Key;
    }
}