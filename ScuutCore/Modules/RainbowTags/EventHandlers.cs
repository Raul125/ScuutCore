using Exiled.API.Extensions;
using PluginAPI.Core;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using UnityEngine;

namespace ScuutCore.Modules.RainbowTags
{
    public class EventHandlers
    {
        private RainbowTags rainbowTags;
        public EventHandlers(RainbowTags rb)
        {
            rainbowTags = rb;
        }

        public void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player is null)
                return;

            bool hasColors = TryGetColors(ev.NewGroup?.GetKey(), out string[] colors);
            if (ev.NewGroup != null && hasColors)
            {
                RainbowTagController controller = ev.Player.GameObject.AddComponent<RainbowTagController>();
                controller.Colors = colors;
                controller.Interval = rainbowTags.Config.TagInterval;
                return;
            }

            if (!ev.Player.GameObject.TryGetComponent(out RainbowTagController rainbowTagController))
                return;

            if (hasColors)
                rainbowTagController.Colors = colors;
            else
                Object.Destroy(rainbowTagController);
        }

        private bool TryGetColors(string rank, out string[] availableColors)
        {
            availableColors = null;
            return !string.IsNullOrEmpty(rank) && rainbowTags.Config.Sequences.TryGetValue(rank, out availableColors);
        }
    }
}