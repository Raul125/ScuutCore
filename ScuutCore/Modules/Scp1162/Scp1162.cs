namespace ScuutCore.Modules.Scp1162
{
    using API.Features;

    public sealed class Scp1162 : EventControllerModule<Config, EventHandlers>
    {
        public static Scp1162 Instance { get; set; }
        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            base.OnDisabled();
        }
    }
}