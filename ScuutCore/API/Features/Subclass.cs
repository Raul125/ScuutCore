namespace ScuutCore.API.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Interfaces;
using ScuutCore.API.Helpers;
using UnityEngine;

public abstract class Subclass
{
    public static List<Subclass> List = new List<Subclass>();

    // Changed later in SerializedSubclass
    public virtual List<Player> GetPlayers() => null;

    /// <summary>
    /// The name of the subclass.
    /// </summary>
    public virtual string Name { get; }

    /// <summary>
    /// The subclasses health.
    /// </summary>
    public virtual float Health { get; } = 100f;

    /// <summary>
    /// The subclasses scale.
    /// </summary>
    public virtual Vector3 Scale { get; } = Vector3.one;

    /// <summary>
    /// Gets the spawn items.
    /// </summary>
    /// <returns>Item array.</returns>
    public virtual ItemType[]? GetSpawnLoadout(Player player) => null;

    /// <summary>
    /// Key is item, value is chance it spawns.
    /// </summary>
    public virtual Dictionary<ItemType, ushort>? GetRandomSpawnItems(Player player) => null;

    /// <summary>
    /// Gets the spawn ammo.
    /// </summary>
    /// <returns>Ammo dict.</returns>
    public virtual Dictionary<ItemType, ushort>? GetAmmoLoadout(Player player) => null;

    /// <summary>
    /// Gets the spawn effects.
    /// </summary>
    public virtual Dictionary<string, byte> GetEffects(Player player) => null;

    /// <summary>
    /// Gets the roles it can replace.
    /// </summary>
    public virtual RoleTypeId[] ToReplace => Array.Empty<RoleTypeId>();

    /// <summary>
    /// Gets the spawn chance.
    /// </summary>
    public virtual float SpawnChance => 0f;

    /// <summary>
    /// Maximum amount of this role that can be alive at the same time.
    /// </summary>
    public virtual int MaxAlive => -1;

    /// <summary>
    /// Maximum amount of this role that can spawn per round.
    /// </summary>
    public virtual int MaxPerRound => -1;

    public virtual void OnLoaded() => Log.Debug($"Subclass {Name} loaded!");

    /// <summary>
    /// Called when someone receives this subclass.
    /// </summary>
    /// <param name="player">The player.</param>
    public virtual void OnReceived(Player player){}

    /// <summary>
    /// Called when someone loses this subclass.
    /// </summary>
    /// <param name="player">The player.</param>
    public virtual void OnLost(Player player){}

    /// <summary>
    /// Gets the spawn message.
    /// </summary>
    /// <returns>The spawn message.</returns>
    public virtual string GetSpawnMessage(Player ply) => $"You are a {Name}!";
}