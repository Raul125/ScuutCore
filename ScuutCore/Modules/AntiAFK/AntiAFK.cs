namespace ScuutCore.Modules.AntiAFK
{
    using API.Features;

    public sealed class AntiAFK : EventControllerModule<AntiAFK, Config, EventHandlers>
    {
        public static AntiAFK Singleton;

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