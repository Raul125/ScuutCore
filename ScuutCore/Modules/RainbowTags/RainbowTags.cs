namespace ScuutCore.Modules.RainbowTags
{
    using API.Features;

    public sealed class RainbowTags : EventControllerModule<RainbowTags, Config, EventHandlers>
    {

        public static RainbowTags Instance;

        public override void OnEnabled()
        {
            base.OnEnabled();
            Instance = this;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Instance = null;
        }

    }
}