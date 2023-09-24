namespace ScuutCore.API.Features;

using System;
using PluginAPI.Core;
using PluginAPI.Core.Factories;
using PluginAPI.Core.Interfaces;

public sealed class ScuutPlayerFactory : PlayerFactory
{
    public override Type BaseType { get; } = typeof(ScuutPlayer);

    public override Player Create(IGameComponent component)
    {
        if (component is ReferenceHub hub && hub.isLocalPlayer)
            return new Server(hub);

        return new ScuutPlayer(component);
    }
}