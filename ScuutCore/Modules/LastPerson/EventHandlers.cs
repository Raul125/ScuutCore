namespace ScuutCore.Modules.LastPerson;

using System.Linq;
using MEC;
using PlayerRoles;
using ScuutCore.API.Features;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
public sealed class EventHandlers : InstanceBasedEventHandler<LastPerson>
{
    [PluginEvent(ServerEventType.PlayerDeath)]
    public void OnDied(Player player, Player attacker, DamageHandlerBase damageHandler)
    {
        if (CheckAlive(out _))
        {
            Timing.CallDelayed(Module.Config.Delay, () =>
            {
                if (CheckAlive(out var ply))
                {
                    ply.ReceiveHint(Module.Config.Message, Module.Config.Duration);
                }
            });
        }
    }

    public bool CheckAlive(out Player ply)
    {
        var allPlayers = Player.GetPlayers();
        ply = null;
        Debug("Started check: ");
        int count = allPlayers.Count(x => x.Role is RoleTypeId.Scientist or RoleTypeId.ClassD);
        Debug(" human (escapable) count: " + count + " is acceptable: " + (count == 1));
        int mtfcount = allPlayers.Count(x => x.IsNTF);
        Debug(" human (ntf) count: " + count + " is acceptable: " + (count != 0));
        int scpcount = allPlayers.Count(x => x.Team == Team.SCPs);
        int chaoscount = allPlayers.Count(x => x.Team == Team.ChaosInsurgency);

        if (count == 1)
        {
            Debug(" escapable count is 1");
            if (mtfcount == 0)
            {
                Debug(" mtf count is 0, running");
                ply = allPlayers.FirstOrDefault(x => x.Role is RoleTypeId.Scientist or RoleTypeId.ClassD);
                return true;
            }

            if(scpcount == 0)
            {
                Debug(" mtf count > 1 and scp count is 0, running");
                ply = allPlayers.FirstOrDefault(x => x.Role is RoleTypeId.Scientist or RoleTypeId.ClassD);
                return true;
            }

            Debug(" mtf count is > 0, scp count is > 0 and escapablecount is 1, returning");
            return false;
        }

        if (mtfcount == 1)
        {
            Debug(" mtf count is 1");
            if (count == 0 && scpcount != 0)
            {
                Debug(" escapable count is 0 and scp is not 0, running");
                ply = allPlayers.FirstOrDefault(x => x.IsNTF);
                return true;
            }

            if(scpcount == 0 && chaoscount > 1)
            {
                Debug(" chaos count > 1 and scp count is 0, running");
                ply = allPlayers.FirstOrDefault(x => x.IsNTF);
                return true;
            }

            Debug(" escapable count is > 0, scp count is > 0 and mtf is 1, returning");
            return false;
        }

        if(scpcount == 1)
        {
            Debug(" scp count is 1");
            if (mtfcount > 1 && chaoscount == 0 && count == 0)
            {
                Debug(" mtf count is > 1 and chaos 0, running");
                ply = allPlayers.FirstOrDefault(x => x.Team == Team.SCPs);
                return true;
            }
        }

        if (chaoscount == 1)
        {
            Debug(" chaos count is 1");
            if (mtfcount > 1 && scpcount == 0)
            {
                Debug(" mtf count > 1, scpcount = - and chaos count is 1, running");
                ply = allPlayers.FirstOrDefault(x => x.Team == Team.ChaosInsurgency);
                return true;
            }
        }

        Debug("Didnt match any filters, returning");
        return false;
    }

    private void Debug(string message)
    {
        if (Module.Config.PrintDebug)
            Log.Debug("LastPerson: " + message);
    }
}