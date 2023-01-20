namespace ScuutCore.Modules.Subclasses.DeclaredSubclasses.Scientist
{
    using System.Collections.Generic;
    using System.Linq;
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using ScuutCore.API.Features;
    using YamlDotNet.Serialization;

    public class HeadResearcher : Subclass
    {
        public override string Name => "Head Researcher";
        public override float Health => 100f;
        public override float SpawnChance => 1f;
        public override int MaxAlive => 1;

        private List<Player> _players = new List<Player>();
        public override List<Player> GetPlayers() => _players;
        public override void OnReceived(Player player)
        {
            _players.Add(player);
            Timing.CallDelayed(3f, () =>
            {
                player.SendBroadcast(
                    $"Scps alive this round: {string.Join(", ", Player.GetPlayers().Where(x => x.IsSCP).Select(x => x.Role.ToString().Replace("SCP", "")))}",
                    8);
            });
        }
        public override void OnLost(Player player) => _players.Remove(player);

        public override ItemType[]? GetSpawnLoadout(Player player) => new []
        {
            ItemType.KeycardScientist,
            ItemType.Medkit,
            ItemType.GunCOM15,
        };
        public override Dictionary<ItemType, ushort>? GetAmmoLoadout(Player player) => new Dictionary<ItemType, ushort>()
        {
            [ItemType.Ammo9x19] = 30,
        };
        
        public override RoleTypeId[] ToReplace => new []
        {
            RoleTypeId.Scientist
        };
    }
}