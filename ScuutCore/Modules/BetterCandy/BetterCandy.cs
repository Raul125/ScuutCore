using ScuutCore.API;

namespace ScuutCore.Modules.BetterCandy
{
    using ScuutCore.API.Features;

    public class BetterCandy : Module<Config>
    {
        public override string Name { get; } = "BetterCandy";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}