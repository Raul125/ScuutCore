namespace ScuutCore.Modules.ChooseRole
{
    using API.Features;

    public sealed class ChooseRole : EventControllerModule<ChooseRole, Config, EventHandlers>
    {
        public static ChooseRole Singleton;

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