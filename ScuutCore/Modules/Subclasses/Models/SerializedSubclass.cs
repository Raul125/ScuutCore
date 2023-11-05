namespace ScuutCore.Modules.Subclasses.Models;

using System.Collections.Generic;
using PlayerRoles;
using PluginAPI.Core;
using ScuutCore.API.Features;
using UnityEngine;
using YamlDotNet.Serialization;

public class SerializedSubclass : Subclass
{
    public virtual string SubclassName { get; set; } = "Name";
    public virtual string SubclassSpawnMessage { get; set; } = "replaceme";
    public virtual float SubclassHealth { get; set; } = 100f;
    public virtual Vector3 SubclassScale { get; set; } = Vector3.one;
    public virtual float SubclassSpawnChance { get; set; } = 0f;
    public virtual int SubclassMaxAlive { get; set; } = 0;
    public virtual int SubclassMaxPerRound { get; set; } = 0;

    public virtual RoleTypeId[] RolesToReplace { get; set; } = new[]
    {
        RoleTypeId.Overwatch
    };

    public virtual ItemType[] SpawnLoadout { get; set; } = new[]
    {
        ItemType.Coin
    };

    public virtual Dictionary<ItemType, ushort> RandomSpawnItems { get; set; } = new Dictionary<ItemType, ushort>
    {
        { ItemType.Flashlight, 50 }
    };

    public virtual Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>
    {
        { ItemType.Ammo9x19, 1 }
    };

    public virtual Dictionary<string, byte> Effects { get; set; } = new Dictionary<string, byte>
    {
        {"Scp207", 4}
    };

    [YamlIgnore]
    public override string Name => SubclassName;
    [YamlIgnore]
    public override float Health => SubclassHealth;
    [YamlIgnore]
    public override Vector3 Scale => SubclassScale;
    [YamlIgnore]
    public override float SpawnChance => SubclassSpawnChance;
    [YamlIgnore]
    public override int MaxAlive => SubclassMaxAlive;
    [YamlIgnore]
    public override int MaxPerRound => SubclassMaxPerRound;

    private List<Player> _players = new List<Player>();
    public override List<Player> GetPlayers() => _players;
    public override void OnReceived(Player player) => _players.Add(player);
    public override void OnLost(Player player) => _players.Remove(player);

    public override ItemType[]? GetSpawnLoadout(Player player) => SpawnLoadout;
    public override Dictionary<ItemType, ushort>? GetRandomSpawnItems(Player player) => RandomSpawnItems;
    public override Dictionary<ItemType, ushort>? GetAmmoLoadout(Player player) => SpawnAmmo;

    public override Dictionary<string, byte> GetEffects(Player player) => Effects;

    [YamlIgnore]
    public override RoleTypeId[] ToReplace => RolesToReplace;
    public override string GetSpawnMessage(Player ply) => string.IsNullOrWhiteSpace(SubclassSpawnMessage) || SubclassSpawnMessage == "replaceme" 
        ? $"You are now a {Name}!" 
        : SubclassSpawnMessage;
}