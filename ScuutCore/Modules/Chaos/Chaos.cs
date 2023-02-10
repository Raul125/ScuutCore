namespace ScuutCore.Modules.Chaos
{
    using API.Features;

    public sealed class Chaos : EventControllerModule<Config, EventHandlers>
    {
        public override string Name => "Chaos";

    }
}