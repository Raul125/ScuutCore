namespace ScuutCore.Modules.Scp096Notifications;

using ScuutCore.API.Features;
public sealed class EventHandlers : InstanceBasedEventHandler<Scp096Notifications>
{
    /*public void OnAddingTarget(AddingTargetEventArgs ev)
    {
        if (ev.Target == null ||
            ev.Target.SessionVariables.ContainsKey("IsNPC") ||
            ev.Player.SessionVariables.ContainsKey("IsNPC"))
            return;

        if (Module.Config.Enable096SeenMessage)
        {
            ev.Target.ShowHint(Module.Config.Scp096SeenMessage, Module.Config.HintDuration);
        }

        if (Module.Config.Enable096NewTargetMessage)
        {
            if (!Module.Config.RoleStrings.TryGetValue(ev.Target.Role.Type, out string translatedRole))
                translatedRole = ev.Target.Role.Type.ToString();

            string message = Module.Config.Scp096NewTargetMessage
                .Replace("$name", ev.Target.Nickname)
                .Replace("$class", $"<color={ev.Target.Role.Color.ToHex()}>{translatedRole}</color>");

            ev.Player.ShowHint(message, Module.Config.HintDuration);
        }
    }*/
}