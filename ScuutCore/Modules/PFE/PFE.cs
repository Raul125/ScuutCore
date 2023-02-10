namespace ScuutCore.Modules.PFE
{
    using API.Features;

    public sealed class PFE : EventControllerModule<Config, EventHandlers>
    {
        public override string Name => "Peanut fucking explodes";

    }
}