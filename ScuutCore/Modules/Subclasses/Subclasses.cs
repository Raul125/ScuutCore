namespace ScuutCore.Modules.Subclasses
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using PluginAPI.Core;
    using PluginAPI.Events;
    using PluginAPI.Helpers;
    using ScuutCore.API.Features;
    using YamlDotNet.Serialization;

    public class Subclasses : Module<Config>
    {
        public override string Name { get; } = "Subclasses";

        public static Subclasses Singleton;
        public static Dictionary<string, string> SpawnTranslations = new Dictionary<string, string>();

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers();
            EventManager.RegisterEvents(this, EventHandlers);

            Log.Warning("Loading subclasses!");
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
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
            }
            
            string yamlFile = Path.Combine(Plugin.Singleton.Config.ConfigsFolder, "subclasstranslations.yaml");
            Dictionary<string, string> deserialized = null;
            try
            {
                if(File.Exists(yamlFile))
                {
                    string subclassConfigs = File.ReadAllText(yamlFile);
                    var deserializer = new Deserializer();
                    deserialized = deserializer.Deserialize<Dictionary<string, string>>(subclassConfigs);
                }
                if (deserialized == null)
                {
                    Dictionary<string, string> toWrite = new Dictionary<string, string>();
                    foreach (var subclass in Subclass.List)
                    {
                        toWrite.Add(subclass.Name, "Youre a " + subclass.Name);
                    }
                    var serializer = new Serializer();
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