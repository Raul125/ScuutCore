namespace ScuutCore.Modules.PocketFall;

using API.Features;

public sealed class PocketFall : EventControllerModule<PocketFall, Config, EventHandlers>
{
    public static PocketFall? Instance { get; private set; }
    public override void OnEnabled()
    {
        base.OnEnabled();
        Instance = this;
    }
    public override void OnDisabled()
    {
        base.OnDisabled();
        Instance = null;
    }
}