using Interactables.Interobjects.DoorUtils;
using PluginAPI.Core;
using System.Linq;
using InventorySystem.Items.Keycards;
using MapGeneration.Distributors;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.RemoteKeycard
{
    public class EventHandlers
    {
        private RemoteKeycard remotekeycard;
        public EventHandlers(RemoteKeycard rm)
        {
            remotekeycard = rm;
        }

        [PluginEvent(ServerEventType.PlayerInteractDoor)]
        public bool OnPlayerInteractDoor(Player ply, DoorVariant door, bool canOpen)
        {
            if (!remotekeycard.Config.IsEnabled || ply.IsSCP() || remotekeycard.Config.BlackListRole.Contains(ply.Role) || ply.IsWithoutItems() ||
                remotekeycard.Config.BlacklistedDoors.Any(d => door.name.StartsWith(d))
                || ply.CurrentItem is KeycardItem) return true;

            if (ply.HasKeycardPermission(door.RequiredPermissions.RequiredPermissions))
            {
                canOpen = true;
                door.Toggle();
                return false;
            }

            return true;
        }

        [PluginEvent(ServerEventType.PlayerInteractLocker)]
        public bool OnPlayerInteractLocker(Player ply, Locker locker, LockerChamber chamber, bool canOpen)
        {
            if (!remotekeycard.Config.IsEnabled || ply.IsSCP() || remotekeycard.Config.BlackListRole.Contains(ply.Role) || ply.IsWithoutItems() ||
                remotekeycard.Config.BlacklistedLockers.Any(l => locker.name.StartsWith(l)) ||
                ply.CurrentItem is KeycardItem) return true;

            if (ply.HasKeycardPermission(chamber.RequiredPermissions, true))
            {
                canOpen = true;
                chamber.IsOpen = !chamber.IsOpen;
                return false;
            }

            return true;
        }
    }
}