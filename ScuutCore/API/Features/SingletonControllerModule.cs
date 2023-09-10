namespace ScuutCore.API.Features
{
    using ScuutCore.API.Interfaces;

    public abstract class SingletonControllerModule<TModule, TModuleConfig> : Module<TModuleConfig>
        where TModule : SingletonControllerModule<TModule, TModuleConfig>
        where TModuleConfig : IModuleConfig, new()
    {
        public static TModule? Singleton { get; private set; }

        public override void OnEnabled()
        {
            Singleton = (TModule)this;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            base.OnDisabled();
        }
    }
}