namespace ScuutCore.Modules.RespawnTimer
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;
    using PluginAPI.Core;
    using System.IO;
    using Serialization;

    public class RespawnTimer : Module<Config>
    {
        public override string Name { get; } = "RespawnTimer";

        private EventHandlers EventHandlers;

        public static RespawnTimer Instance;
        public static string RespawnTimerDirectoryPath { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            RespawnTimerDirectoryPath = PluginHandler.Get(this).PluginDirectoryPath;

            if (!Directory.Exists(RespawnTimerDirectoryPath))
            {
                Log.Warning("RespawnTimer directory does not exist. Creating...");
                Directory.CreateDirectory(RespawnTimerDirectoryPath);
            }

            string templateDirectory = Path.Combine(RespawnTimerDirectoryPath, "Template");
            if (!Directory.Exists(templateDirectory))
            {
                Directory.CreateDirectory(templateDirectory);

                File.Create(Path.Combine(templateDirectory, "TimerBeforeSpawn.txt"));
                File.Create(Path.Combine(templateDirectory, "TimerDuringSpawn.txt"));
                File.WriteAllText(Path.Combine(templateDirectory, "Properties.yml"), YamlParser.Serializer.Serialize(new Properties()));

                string hintsPath = Path.Combine(templateDirectory, "Hints.txt");
                File.WriteAllText(hintsPath, "This is an example hint. You can add as much as you want.");
            }

            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(this, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}