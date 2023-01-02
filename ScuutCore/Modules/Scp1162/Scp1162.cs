namespace ScuutCore.Modules.Scp1162
{
    using PluginAPI.Events;
    using ScuutCore.API;

    public class Scp1162 : Module<Config>
    {
        public override string Name { get; } = "Scp1162";
        public static Scp1162 Instance { get; set; }

        public override void OnEnabled()
        {
            Instance = this;
            EventManager.RegisterEvents<EventHandlers>(this);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents<EventHandlers>(this);
            Instance = null;
            base.OnDisabled();
        }
    }
}