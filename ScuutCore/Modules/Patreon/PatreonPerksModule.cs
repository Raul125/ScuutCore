namespace ScuutCore.Modules.Patreon
{
    using API.Features;
    public sealed class PatreonPerksModule : EventControllerModule<PatreonPerksModule, Config, EventHandlers>
    {
        public static PatreonPerksModule Singleton { get; private set; }

        public override void OnEnabled()
        {
            base.OnEnabled();
            Singleton = this;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Singleton = null;
        }
    }
}