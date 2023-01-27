namespace ScuutCore.Modules.Subclasses
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Events;
    using PluginAPI.Helpers;
    using ScuutCore.API.Features;
    using ScuutCore.Modules.Subclasses.Models;
    using YamlDotNet.Serialization;

    public class Subclasses : Module<Config>
    {
        public override string Name { get; } = "Subclasses";

        public static Subclasses Singleton;
        public static Dictionary<string, string> SpawnTranslations = new();

        private EventHandlers EventHandlers;
        
        private SerializedSubclass[] defaultSubclassesValue = new SerializedSubclass[]
        {
            new SerializedSubclass()
            {
                SubclassName = "Janitor",
                SubclassSpawnChance = 15f,
                SubclassMaxAlive = 2,
                SubclassMaxPerRound = 0,
                SpawnLoadout = new []
                {
                    ItemType.KeycardJanitor
                },
                RolesToReplace = new []
                {
                    RoleTypeId.ClassD
                }
            }
        };

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers();
            EventManager.RegisterEvents(this, EventHandlers);

            Log.Warning("Loading subclasses!");
            var serializer = new Serializer();
            var deserializer = new Deserializer();
            List<SerializedSubclass> serializedSubclasses = new List<SerializedSubclass>();
            if (Directory.Exists(Config.SubclassFolder))
            {
                foreach (var file in Directory.GetFiles(Config.SubclassFolder, "*.yml", SearchOption.AllDirectories))
                {
                    if (file is null)
                        continue;

                    try
                    {
                        if (file.Contains("list"))
                            serializedSubclasses.AddRange(deserializer.Deserialize<List<SerializedSubclass>>(File.ReadAllText(file)));
                        else
                            serializedSubclasses.Add(deserializer.Deserialize<SerializedSubclass>(File.ReadAllText(file)));
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Error parsing file {file}: {e}");
                    }
                }
            }

            if (serializedSubclasses.Count == 0)
            {
                Directory.CreateDirectory(Config.SubclassFolder);
                File.WriteAllText(Path.Combine(Config.SubclassFolder, "exampleclass.yml"),
                serializer.Serialize(defaultSubclassesValue[0]));
                File.WriteAllText(Path.Combine(Config.SubclassFolder, "exampleclasses.list.yml"), serializer.Serialize(defaultSubclassesValue));
            }
            else
            {
                foreach (var serializedSubclass in serializedSubclasses)
                {
                    Subclass.List.Add(serializedSubclass);
                    serializedSubclass.OnLoaded();
                }
            }
            /*foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if(type.Namespace != null && !type.Namespace.StartsWith("ScuutCore.Modules.Subclasses.DeclaredSubclasses"))
                {
                    continue;
                }

                if (type.BaseType != typeof(Subclass))
                {
                    continue;
                }
                
                var subclass = (Subclass)Activator.CreateInstance(type);
                subclass.OnLoaded();
            }*/
            foreach (var property in Config.GetType().GetProperties())
            {
                if (property.PropertyType.BaseType != typeof(Subclass) && property.PropertyType.BaseType != typeof(SerializedSubclass))
                    continue;

                var subclass = property.GetValue(Config);
                if (subclass is Subclass subclass2)
                {
                    Subclass.List.Add(subclass2);
                    subclass2.OnLoaded();
                }
            }
            
            string yamlFile = Path.Combine(Plugin.Singleton.Config.ConfigsFolder, "subclasstranslations.yml");
            Dictionary<string, string> deserialized = null;
            try
            {
                if (File.Exists(yamlFile))
                {
                    string subclassConfigs = File.ReadAllText(yamlFile);
                    deserialized = deserializer.Deserialize<Dictionary<string, string>>(subclassConfigs);
                }

                if (deserialized is null)
                {
                    Dictionary<string, string> toWrite = new();
                    foreach (var subclass in Subclass.List)
                        toWrite.Add(subclass.Name, "Youre a " + subclass.Name);

                    File.WriteAllText(Path.Combine(Paths.Configs, yamlFile), serializer.Serialize(toWrite));
                }

                SpawnTranslations = deserialized;
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while deserializing the subclass translations file: " + e);
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;
            Singleton = null;

            base.OnDisabled();
        }
    }
}