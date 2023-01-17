namespace ScuutCore.Modules.Subclasses.DeclaredSubclasses.ClassD
{
    using System.Collections.Generic;
    using PlayerRoles;
    using PluginAPI.Core;
    using ScuutCore.API.Features;

    public class Janitor : Subclass
    {
        public override string Name { get; } = "Janitor";
        public override float SpawnChance { get; } = 15f;
        public override int MaxAlive { get; } = 2;
        public override RoleTypeId[] ToReplace { get; } = new[] { RoleTypeId.ClassD };

        private List<Player> _players = new List<Player>();
        public override List<Player> GetPlayers() => _players;
        public override void OnReceived(Player player) => _players.Add(player);
        public override void OnLost(Player player) => _players.Remove(player);

        public override ItemType[]? GetSpawnLoadout(Player player) => new[]
        {
            ItemType.KeycardJanitor
        };
    }
}