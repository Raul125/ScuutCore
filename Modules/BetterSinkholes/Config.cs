using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.BetterSinkholes
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public float TeleportDistance { get; set; } = 2f;

        [Description("The number of ticks before teleporting player to the pocket dimension.")]
        public uint Ticks { get; set; } = 350;

        [Description("Thea amount of damage to deal to someone when they fall into the pocket dimension")]
        public float EntranceDamage { get; set; } = 40;

        public float PositionChange { get; set; } = 0.0001f;

        [Description("The message to show to someone when they fall into the pocket dimension.")]
        public Exiled.API.Features.Broadcast TeleportMessage { get; set; } = new Exiled.API.Features.Broadcast
        {
            Duration = 5,
            Content = "You've fallen into the pocket dimension!",
            Show = false,
            Type = Broadcast.BroadcastFlags.Normal
        };
    }
}