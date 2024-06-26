﻿namespace ScuutCore.Modules.RemoteKeycard;

using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Keycards;
using MapGeneration.Distributors;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Interfaces;
public static class Extension
{
    /// <summary>
    /// Checks whether the player has a keycard of a specific permission.
    /// </summary>
    /// <param name="player"><see cref="IPlayer"/> trying to interact.</param>
    /// <param name="permissions">The permission that's gonna be searched for.</param>
    /// <param name="requiresAllPermissions">Whether all permissions are required. Use this for chambers</param>
    /// <returns>Whether the player has the required keycard.</returns>
    public static bool HasKeycardPermission(this Player player, KeycardPermissions permissions, bool requiresAllPermissions = false)
    {
        try
        {
            if (player.EffectsManager.TryGetEffect<CustomPlayerEffects.AmnesiaItems>(out var effect) && effect.IsEnabled)
            {
                return false;
            }

            foreach (var item in player.ReferenceHub.inventory.UserInventory.Items.Values)
            {
                if (item is not KeycardItem keycard)
                    continue;
                return requiresAllPermissions ? keycard.Permissions.HasFlagFast(permissions) : (keycard.Permissions & permissions) != 0;
            }
            return false;
        }
        catch (System.Exception e)
        {
            Log.Error($"Error on HasKeycardPermission: {e.Message} ---- {e.StackTrace}");
            return false;
        }
    }

    /// <summary>
    /// Open or Close a <see cref="DoorVariant"/>
    /// </summary>
    /// <param name="door"> It's a door, what else do you expect?</param>
    public static void Toggle(this DoorVariant door)
    {
        door.NetworkTargetState = !door.NetworkTargetState;
    }

    /// <summary>
    /// Open or Close this <see cref="Locker"/>
    /// </summary>
    /// <param name="chamber"></param>
    /// <param name="locker"></param>
    public static void Toggle(this LockerChamber chamber, Locker locker)
    {
        chamber.SetDoor(!chamber.IsOpen, locker._grantedBeep);
        locker.RefreshOpenedSyncvar();
    }

    /// <summary>
    /// Check if a Itemtype is SCP Item
    /// </summary>
    /// <returns>true if ItemType is SCP</returns>
    public static bool IsSCP(this ItemType type) => type is ItemType.SCP018 or ItemType.SCP500 or ItemType.SCP268 or ItemType.SCP207 or ItemType.SCP244a or ItemType.SCP244b or ItemType.SCP2176 or ItemType.SCP1853;

    /// <summary>
    /// Check if the player has no items in is inventory.
    /// </summary>
    /// <returns>true if player no have items</returns>
    public static bool IsWithoutItems(this Player ply) =>
        ply.ReferenceHub.inventory.UserInventory.Items.Count == 0;

    /// <summary>
    /// Check if the player is any SCP.
    /// </summary>
    /// <returns>true if player is SCP</returns>
    public static bool IsSCP(this Player ply, bool include3314 = false) => ply.Role is RoleTypeId.Scp049 or RoleTypeId.Scp079
        or RoleTypeId.Scp096 or RoleTypeId.Scp106 or RoleTypeId.Scp173 or RoleTypeId.Scp0492 or RoleTypeId.Scp939 || (include3314 && ply.Role == RoleTypeId.Scp3114);
}