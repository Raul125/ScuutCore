namespace ScuutCore.Modules.PFE
{
    using ScuutCore.API.Features;

    public sealed class PFE : EventControllerModule<PFE, Config, EventHandlers>
    {
        public override string Name => "Peanut fucking explodes";

    }
}