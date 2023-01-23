namespace ScuutCore.Modules.Subclasses.DeclaredSubclasses.Scientist
{
    using System.Collections.Generic;
    using System.Linq;
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using ScuutCore.API.Features;
    using ScuutCore.Modules.Subclasses.Models;

    public class HeadResearcher : SerializedSubclass
    {
        public override string SubclassName => "Head Researcher";
        public override float SubclassHealth => 100f;
        public override float SubclassSpawnChance => 1f;
        public override int SubclassMaxAlive => 1;
        public override int SubclassMaxPerRound => 0;

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

        public override ItemType[] SpawnLoadout => new []
        {
            ItemType.KeycardScientist,
            ItemType.Medkit,
            ItemType.GunCOM15,
        };
        public override Dictionary<ItemType, ushort> SpawnAmmo => new Dictionary<ItemType, ushort>()
        {
            [ItemType.Ammo9x19] = 30,
        };
        
        public override RoleTypeId[] RolesToReplace => new []
        {
            RoleTypeId.Scientist
        };
    }
}