using Exiled.API.Extensions;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ScuutCore.API
{
    public static class Loader
    {
        public static YamlDotNet.Serialization.ISerializer Serializer = Exiled.Loader.Loader.Serializer;
        public static YamlDotNet.Serialization.IDeserializer Deserializer = Exiled.Loader.Loader.Deserializer;
        public static SortedSet<IModule<IModuleConfig>> Modules { get; } = new SortedSet<IModule<IModuleConfig>>();
        public static SortedSet<IModule<IModuleConfig>> ActiveModules { get; } = new SortedSet<IModule<IModuleConfig>>();
        
        public static void InitModules()
        {
            Directory.CreateDirectory(Plugin.Singleton.Config.ConfigsFolder);

            foreach (var mod in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (mod.IsAbstract || mod.IsInterface)
                {
                    continue;
                }

                if (!mod.BaseType.IsGenericType || (mod.BaseType.GetGenericTypeDefinition() != typeof(Module<>)))
                {
                    continue;
                }
                    
                IModule<IModuleConfig> module = null;

                try
                {
                    module = CreateModule(mod);
                }
                catch (Exception)
                {
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
            }
            catch (Exception)
            {
                Log.Error($"{module.Name} configs could not be loaded, check them with a yaml validator");
                return;
            }

            module.Config.CopyProperties(cfg);

            if (module.Config.IsEnabled)
            {
                File.WriteAllText(configPath, Serializer.Serialize(module.Config));
                module.OnEnabled();
                ActiveModules.Add(module);
            }
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
}