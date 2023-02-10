namespace ScuutCore.Modules.AdminTools
{
    using API.Features;
    public sealed class AdminTools : EventControllerModule<Config, EventHandlers>
    {
        public override string Name => "AdminTools";

    }
}