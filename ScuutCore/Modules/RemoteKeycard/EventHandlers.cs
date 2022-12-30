using CustomPlayerEffects;
using Interactables.Interobjects.DoorUtils;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Linq;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;

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

            if (!ev.Door.RequiredPermissions.CheckPermissions(ev.Player.CurrentItem?.Base, ev.Player.ReferenceHub) && HasKeycardPermission(ev.Player, ev.Door.RequiredPermissions.RequiredPermissions))
                ev.Door.IsOpen = !ev.Door.IsOpen;
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
                ev.Generator.IsOpen = !ev.Generator.IsOpen;
        }

        public void OnLockerInteract(InteractingLockerEventArgs ev)
        {
            if (!remotekeycard.Config.AffectScpLockers)
                return;

            if (!ev.IsAllowed && ev.Chamber != null && HasKeycardPermission(ev.Player, ev.Chamber.RequiredPermissions, true))
                ev.Chamber.IsOpen = !ev.Chamber.IsOpen;
        }

        // Extension
        public bool HasKeycardPermission(Player player, KeycardPermissions permissions, bool requiresAllPermissions = false)
        {
            if (remotekeycard.Config.AmnesiaMatters && player.IsEffectActive<AmnesiaVision>())
                return false;

            return requiresAllPermissions ?
                player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlagFast(permissions))
                : player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & permissions) != 0);
        }
    }
}