namespace ScuutCore.Modules.Scp1162
{
    using PluginAPI.Events;
    using ScuutCore.API;

    public class Scp1162 : Module<Config>
    {
        public override string Name { get; } = "Scp1162";
        public static Scp1162 Instance { get; set; }
        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers();
            EventManager.RegisterEvents(this, EventHandlers);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            Instance = null;
            EventHandlers = null;
            base.OnDisabled();
        }
    }
}