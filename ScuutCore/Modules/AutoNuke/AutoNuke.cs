namespace ScuutCore.Modules.AutoNuke;

using API.Features;

public sealed class AutoNuke : EventControllerModule<AutoNuke, Config, EventHandlers>
{
    public static AutoNuke Instance;

    public override void OnEnabled()
    {
        Instance = this;
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Instance = null;
        base.OnDisabled();
    }
}