namespace ScuutCore.Modules.ScpSwap
{
    using ScuutCore.API.Features;

    public sealed class ScpSwap : EventControllerModule<ScpSwap, Config, EventHandlers>
    {
        public static ScpSwap Singleton;

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