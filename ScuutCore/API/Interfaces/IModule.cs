namespace ScuutCore.API.Interfaces;

using System;

public interface IModule<out TModuleConfig> : IComparable<IModule<IModuleConfig>>
where TModuleConfig : IModuleConfig
{
    string Name { get; }

    TModuleConfig Config { get; }

    void OnEnabled();

    void OnDisabled();
}