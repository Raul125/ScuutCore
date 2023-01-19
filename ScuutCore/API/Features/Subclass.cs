namespace ScuutCore.API.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PlayerRoles;
    using PluginAPI.Core;
    using ScuutCore.API.Helpers;

    public abstract class Subclass
    {
        public static List<Subclass> List = new List<Subclass>();
        public virtual List<Player> GetPlayers() => Player.GetPlayers().Where(x => x.GetSubclass() != null).ToList();
        
        /// <summary>
        /// The name of the subclass.
        /// </summary>
        public virtual string Name { get; }
        
        /// <summary>
        /// Gets the spawn items.
        /// </summary>
        /// <returns>Item array.</returns>
        public virtual ItemType[]? GetSpawnLoadout(Player player) => null;

        /// <summary>
        /// Gets the spawn ammo.
        /// </summary>
        /// <returns>Ammo dict.</returns>
        public virtual Dictionary<ItemType, ushort>? GetAmmoLoadout(Player player) => null;

        /// <summary>
        /// Gets the roles it can replace.
        /// </summary>
        public virtual RoleTypeId[] ToReplace => Array.Empty<RoleTypeId>();

        /// <summary>
        /// Gets the spawn chance.
        /// </summary>
        public virtual float SpawnChance => 0f;
        
        /// <summary>
        /// Maximum amount of this role that can spawn.
        /// </summary>
        public virtual int MaxAlive => 0;

        public virtual void OnLoaded() => Log.Debug($"Subclass {Name} loaded!");
        
        public Subclass() => List.Add(this);
        
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
    }
}