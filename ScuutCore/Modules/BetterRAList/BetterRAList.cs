using ScuutCore.API;

namespace ScuutCore.Modules.BetterRAList
{
    public class BetterRAList : Module<Config>
    {
        public override string Name { get; } = "BetterRAList";

        public static BetterRAList Singleton;

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