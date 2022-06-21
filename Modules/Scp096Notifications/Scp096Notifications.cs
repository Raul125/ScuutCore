using ScuutCore.API;
using Scp096 = Exiled.Events.Handlers.Scp096;

namespace ScuutCore.Modules.Scp096Notifications
{
    public class Scp096Notifications : Module<Config>
    {
        public override string Name { get; } = "Scp096Notifications";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Scp096.AddingTarget += EventHandlers.OnAddingTarget;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Scp096.AddingTarget -= EventHandlers.OnAddingTarget;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}