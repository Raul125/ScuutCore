namespace ScuutCore.Modules.Subclasses.Models
{
    using System.Collections.Generic;
    using PlayerRoles;
    using PluginAPI.Core;
    using ScuutCore.API.Features;
    using YamlDotNet.Serialization;

    public class SerializedSubclass : Subclass
    {
        public string SubclassName { get; set; } = "Name";
        public float SubclassSpawnChance { get; set; } = 0f;
        public int SubclassMaxAlive { get; set; } = 0;
        
        public RoleTypeId[] RolesToReplace { get; set; } = new[]
        {
            RoleTypeId.Overwatch
        };

        public ItemType[] SpawnLoadout { get; set; } = new[]
        {
            ItemType.Coin
        };

        [YamlIgnore]
        public override string Name => SubclassName;
        [YamlIgnore]
        public override float SpawnChance => SubclassSpawnChance;
        [YamlIgnore]
        public override int MaxAlive => SubclassMaxAlive;

        private List<Player> _players = new List<Player>();
        public override List<Player> GetPlayers() => _players;
        public override void OnReceived(Player player) => _players.Add(player);
        public override void OnLost(Player player) => _players.Remove(player);

        public override ItemType[]? GetSpawnLoadout(Player player) => SpawnLoadout;

        [YamlIgnore]
        public override RoleTypeId[] ToReplace => RolesToReplace;
    }
}