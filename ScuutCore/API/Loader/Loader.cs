namespace ScuutCore.API.Loader;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Interfaces;
using PluginAPI.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public static class Loader
{
    public static ISerializer Serializer { get; } = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreFields()
        .Build();

    public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreFields()
        .IgnoreUnmatchedProperties()
        .Build();

    public static SortedSet<IModule<IModuleConfig>> Modules { get; } = new SortedSet<IModule<IModuleConfig>>();
    public static SortedSet<IModule<IModuleConfig>> ActiveModules { get; } = new SortedSet<IModule<IModuleConfig>>();

    public static void InitModules()
    {
        Directory.CreateDirectory(Plugin.Singleton.Config.ConfigsFolder);
        foreach (var mod in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (mod.IsAbstract || mod.IsInterface || !mod.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IModule<>)))
                continue;

            IModule<IModuleConfig> module;

            try
            {
                module = CreateModule(mod);
            }
            catch (Exception e)
            {
                Log.Error($"Failed loading module {mod.FullName}:\n" + e);
                continue;
            }

            if (module == null)
                continue;

            Modules.Add(module);
            StartModule(module);
        }
    }

    private static void StartModule(IModule<IModuleConfig> module)
    {
        // Get config from files
        var directroyPath = Path.Combine(Plugin.Singleton.Config.ConfigsFolder, module.Name);
        Directory.CreateDirectory(directroyPath);

        var configPath = Path.Combine(directroyPath, "Config.yml");
        if (!File.Exists(configPath))
            File.WriteAllText(configPath, Serializer.Serialize(module.Config));

        IModuleConfig cfg;

        try
        {
            cfg = (IModuleConfig)Deserializer.Deserialize(File.ReadAllText(configPath), module.Config.GetType());
            CopyProperties(module.Config, cfg);
        }
        catch (Exception)
        {
            Log.Error($"{module.Name} configs could not be loaded, replacing with default config");
        }

        if (module.Config.IsEnabled)
        {
            File.WriteAllText(configPath, Serializer.Serialize(module.Config));
            module.OnEnabled();
            ActiveModules.Add(module);
        }
    }

    private static void CopyProperties(object target, object source)
    {
        Type type = target.GetType();
        if (type != source.GetType())
            throw new InvalidOperationException("Target and source type mismatch!");

        foreach (PropertyInfo sourceProperty in type.GetProperties())
            type.GetProperty(sourceProperty.Name)?.SetValue(target, sourceProperty.GetValue(source, null), null);
    }

    public static void StopModules()
    {
        foreach (var module in ActiveModules)
            module.OnDisabled();

        ActiveModules.Clear();
    }

    private static IModule<IModuleConfig> CreateModule(Type type)
    {
        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
        IModule<IModuleConfig> module = null;

        // Actually don't know what this part of code is doing, but it's working as expected
        if (constructor != null)
        {
            module = constructor.Invoke(null) as IModule<IModuleConfig>;
        }
        else
        {
            object value = Array.Find(type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public), property => property.PropertyType == type)?.GetValue(null);

            if (value != null)
                module = value as IModule<IModuleConfig>;
        }

        return module;
    }
}