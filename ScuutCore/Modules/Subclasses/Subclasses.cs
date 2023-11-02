namespace ScuutCore.Modules.Subclasses;

using System;
using System.Collections.Generic;
using System.IO;
using ScuutCore.API.Features;
using Models;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Helpers;
using YamlDotNet.Serialization;

public sealed class Subclasses : EventControllerModule<Subclasses, Config, EventHandlers>
{
    private readonly SerializedSubclass[] defaultSubclassesValue =
    {
        new SerializedSubclass
        {
            SubclassName = "Janitor",
            SubclassSpawnChance = 15f,
            SubclassMaxAlive = 2,
            SubclassMaxPerRound = 0,
            SpawnLoadout = new[]
            {
                ItemType.KeycardJanitor
            },
            RolesToReplace = new[]
            {
                RoleTypeId.ClassD
            }
        }
    };

    public override void OnEnabled()
    {
        Log.Warning("Loading subclasses!");
        var serializer = new Serializer();
        var deserializer = new Deserializer();
        List<SerializedSubclass> serializedSubclasses = new();
        if (Directory.Exists(Config.SubclassFolder))
        {
            foreach (var file in Directory.GetFiles(Config.SubclassFolder, "*.yml", SearchOption.AllDirectories))
            {
                if (file is null)
                    continue;

                try
                {
                    if (file.Contains("list"))
                    {
                        var deserialized = deserializer.Deserialize<List<SerializedSubclass>>(File.ReadAllText(file));
                        File.WriteAllText(file, serializer.Serialize(deserialized));
                        serializedSubclasses.AddRange(deserialized);
                    }
                    else
                    {
                        var deserialized = deserializer.Deserialize<SerializedSubclass>(File.ReadAllText(file));
                        File.WriteAllText(file, serializer.Serialize(deserialized));
                        serializedSubclasses.Add(deserialized);
                    }
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
        Subclass.List.Add(Config.HeadResearcher);
        Config.HeadResearcher.OnLoaded();

        base.OnEnabled();
    }
}