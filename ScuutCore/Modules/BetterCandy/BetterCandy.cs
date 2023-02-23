namespace ScuutCore.Modules.BetterCandy
{
    using API.Features;

    public sealed class BetterCandy : Module<Config>
    {
        public static BetterCandy Singleton;

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