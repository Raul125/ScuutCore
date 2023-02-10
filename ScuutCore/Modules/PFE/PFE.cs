namespace ScuutCore.Modules.PFE
{
    using API.Features;

    public sealed class PFE : EventControllerModule<PFE, Config, EventHandlers>
    {
        public override string Name => "Peanut fucking explodes";

    }
}