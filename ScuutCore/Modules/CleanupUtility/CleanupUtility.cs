namespace ScuutCore.Modules.CleanupUtility
{
    using API.Features;

    public sealed class CleanupUtility : EventControllerModule<CleanupUtility, Config, EventHandlers>
    {
        public static CleanupUtility Singleton { get; private set; }
        public override void OnEnabled()
        {
            Singleton = this;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            base.OnDisabled();
        }
    }
}