namespace ScuutCore.Modules.NukeRadiation
{
    using ScuutCore.API.Features;

    public sealed class NukeRadiation : EventControllerModule<NukeRadiation, Config, EventHandlers>
    {
        public static NukeRadiation Singleton;
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