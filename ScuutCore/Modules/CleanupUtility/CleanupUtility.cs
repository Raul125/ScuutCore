namespace ScuutCore.Modules.CleanupUtility
{
    using ScuutCore.API.Features;

    public class CleanupUtility : Module<Config>
    {
        public override string Name { get; } = "CleanupUtility";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            //EventManager.RegisterEvents(this, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            //EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}