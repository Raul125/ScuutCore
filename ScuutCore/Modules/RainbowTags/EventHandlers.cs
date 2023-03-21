namespace ScuutCore.Modules.RainbowTags
{
    using System.Linq;
    using API.Features;
    using MEC;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<RainbowTags>
    {
        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnJoined(Player player)
        {
            if (player?.GameObject == null)
                return;

            Plugin.Coroutines.Add(Timing.CallDelayed(2f, () =>
            {
                bool hasColors = TryGetColors(GetKey(player.ReferenceHub.serverRoles.Group), out string[] colors);
                if (player.ReferenceHub.serverRoles.Group != null && hasColors)
                {
                    var controller = player.GameObject.AddComponent<RainbowTagController>();
                    controller.Colors = colors;
                    controller.Interval = Module.Config.TagInterval;
                    return;
                }

                if (!player.GameObject.TryGetComponent(out RainbowTagController rainbowTagController))
                    return;

                if (hasColors)
                    rainbowTagController.Colors = colors;
                else
                    Object.Destroy(rainbowTagController);
            }));
        }

        private bool TryGetColors(string rank, out string[] availableColors)
        {
            availableColors = null;
            return !string.IsNullOrEmpty(rank) && Module.Config.Sequences.TryGetValue(rank, out availableColors);
        }

        public static string GetKey(UserGroup group) => ServerStatic.PermissionsHandler._groups
            .FirstOrDefault(pair => pair.Value.Equals(group)).Key;
    }
}