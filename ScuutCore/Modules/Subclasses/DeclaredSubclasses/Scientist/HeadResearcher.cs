namespace ScuutCore.Modules.Subclasses.DeclaredSubclasses.Scientist;

using System.Collections.Generic;
using System.Linq;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using ScuutCore.Modules.Subclasses.Models;

public class HeadResearcher : SerializedSubclass
{
    public override string SubclassName { get; set; } = "Head Researcher";
    public override float SubclassHealth { get; set; } = 100f;
    public override float SubclassSpawnChance { get; set; } = 1f;
    public override int SubclassMaxAlive { get; set; } = 1;
    public override int SubclassMaxPerRound { get; set; } = 0;
    public string SpawnHint { get; set; } = "Scps alive this round: %scps%";

    private List<Player> _players = new();
    public override List<Player> GetPlayers() => _players;
    public override void OnReceived(Player player)
    {
        _players.Add(player);
        Plugin.Coroutines.Add(Timing.CallDelayed(3f, () =>
        {
            player.SendBroadcast(
                SpawnHint.Replace("%scps%", string.Join(", ", Player.GetPlayers().Where(x => x.IsSCP).Select(x => x.Role.ToString()))),
                8);
        }));
    }

    public override void OnLost(Player player) => _players.Remove(player);

    public override ItemType[] SpawnLoadout { get; set; } = new []
    {
        ItemType.KeycardScientist,
        ItemType.Medkit,
        ItemType.GunCOM15,
    };
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 30,
    };
    
    public override RoleTypeId[] RolesToReplace { get; set; } = new []
    {
        RoleTypeId.Scientist
    };
}