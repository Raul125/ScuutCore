namespace ScuutCore.Modules.Teslas
{
    using ScuutCore.API.Features;

    public sealed class Teslas : Module<Config>
    {
        public static Teslas? Singleton;

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