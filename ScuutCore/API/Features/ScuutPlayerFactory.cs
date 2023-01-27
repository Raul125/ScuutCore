namespace ScuutCore.API.Features
{
    using System;
    using PluginAPI.Core.Factories;
    using PluginAPI.Core.Interfaces;

    public class ScuutPlayerFactory : PlayerFactory
    {
        public override Type BaseType { get; } = typeof(ScuutPlayer);

        public override IPlayer Create(IGameComponent component) => new ScuutPlayer(component);
    }
}