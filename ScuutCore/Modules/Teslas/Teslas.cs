using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.Teslas
{
    public class Teslas : Module<Config>
    {
        public override string Name { get; } = "Teslas";

        public override void OnEnabled()
        {
            EventManager.RegisterEvents<EventHandlers>(this);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents<EventHandlers>(this);
            base.OnDisabled();
        }
    }
}