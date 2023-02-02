namespace ScuutCore.Modules.BetterCandy
{
    using ScuutCore.API.Features;

    public class BetterCandy : Module<Config>
    {
        public override string Name { get; } = "BetterCandy";

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