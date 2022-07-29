using CustomPlayerEffects;
using Interactables.Interobjects.DoorUtils;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Linq;
using Exiled.API.Features.Items;

namespace ScuutCore.Modules.RemoteKeycard
{
    public class EventHandlers
    {
        private RemoteKeycard remotekeycard;
        public EventHandlers(RemoteKeycard rm)
        {
            remotekeycard = rm;
        }

        public void OnDoorInteract(InteractingDoorEventArgs ev)
        {
            if (!remotekeycard.Config.AffectDoors)
                return;

            if (!ev.IsAllowed && HasKeycardPermission(ev.Player, ev.Door.RequiredPermissions.RequiredPermissions))
                ev.IsAllowed = true;
        }

        public void OnWarheadUnlock(ActivatingWarheadPanelEventArgs ev)
        {
            if (!remotekeycard.Config.AffectWarheadPanel)
                return;

            if (!ev.IsAllowed && HasKeycardPermission(ev.Player, KeycardPermissions.AlphaWarhead))
                ev.IsAllowed = true;
        }

        public void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
        {
            if (!remotekeycard.Config.AffectGenerators)
                return;

            if (!ev.IsAllowed && HasKeycardPermission(ev.Player, ev.Generator.Base._requiredPermission))
                ev.IsAllowed = true;
        }

        public void OnLockerInteract(InteractingLockerEventArgs ev)
        {
            if (!remotekeycard.Config.AffectScpLockers)
                return;

            if (!ev.IsAllowed && ev.Chamber != null && HasKeycardPermission(ev.Player, ev.Chamber.RequiredPermissions, true))
                ev.IsAllowed = true;
        }

        // Extension
        public bool HasKeycardPermission(Player player, KeycardPermissions permissions, bool requiresAllPermissions = false)
        {
            if (remotekeycard.Config.AmnesiaMatters && player.GetEffectActive<Amnesia>())
                return false;

            return requiresAllPermissions ?
                player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlagFast(permissions))
                : player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & permissions) != 0);
        }
    }
}