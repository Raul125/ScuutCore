namespace ScuutCore.Modules.RemoteKeycard
{
    using System.Linq;
    using API.Features;
    using Interactables.Interobjects.DoorUtils;
    using InventorySystem.Items.Keycards;
    using MapGeneration.Distributors;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public sealed class EventHandlers : InstanceBasedEventHandler<RemoteKeycard>
    {
        [PluginEvent(ServerEventType.PlayerInteractDoor)]
        public bool OnPlayerInteractDoor(Player ply, DoorVariant door, bool canOpen)
        {
            if (!Module.Config.IsEnabled || ply.IsSCP() || Module.Config.BlackListRole.Contains(ply.Role) || ply.IsWithoutItems() ||
                Module.Config.BlacklistedDoors.Any(d => door.name.StartsWith(d))
                || ply.CurrentItem is KeycardItem) return true;

            if (!ply.HasKeycardPermission(door.RequiredPermissions.RequiredPermissions))
                return true;
            canOpen = true;
            door.Toggle();
            return false;

        }

        [PluginEvent(ServerEventType.PlayerInteractLocker)]
        public bool OnPlayerInteractLocker(Player ply, Locker locker, LockerChamber chamber, bool canOpen)
        {
            if (!Module.Config.IsEnabled || ply.IsSCP() || Module.Config.BlackListRole.Contains(ply.Role) || ply.IsWithoutItems() ||
                Module.Config.BlacklistedLockers.Any(l => locker.name.StartsWith(l)) ||
                ply.CurrentItem is KeycardItem) return true;

            if (!ply.HasKeycardPermission(chamber.RequiredPermissions, true))
                return true;
            canOpen = true;
            chamber.Toggle(locker);
            return false;

        }
    }
}