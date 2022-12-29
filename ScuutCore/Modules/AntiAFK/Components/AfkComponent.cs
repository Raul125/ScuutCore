using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Permissions.Extensions;
using PlayerRoles;
using UnityEngine;

namespace ScuutCore.Modules.AntiAFK
{
    public class AfkComponent : MonoBehaviour
    {
        private const float Interval = 1f;
        private float timer;

        private Player player;
        private PositionInfo lastPosition;
        private int afkTime;
        private int afkCounter;

        private void Awake()
        {
            player = Player.Get(gameObject);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= Interval)
            {
                timer = 0f;
                CheckAfk();
            }
        }

        private void CheckAfk()
        {
            if (player.CheckPermission("ScuutCore.afkignore") ||
                player.IsDead ||
                AntiAFK.Singleton.Config.MinimumPlayers > Player.Dictionary.Count ||
                (AntiAFK.Singleton.Config.IgnoreTutorials && player.Role.Type == RoleTypeId.Tutorial) ||
                player.Role is Scp096Role rol && rol.TryingNotToCry)
            {
                afkTime = 0;
                return;
            }

            PositionInfo currentPosition = new PositionInfo(player);
            if (currentPosition != lastPosition)
            {
                afkTime = 0;
                lastPosition = currentPosition;
                return;
            }

            afkTime++;
            if (afkTime < AntiAFK.Singleton.Config.AfkTime)
                return;

            int gracePeriodRemaining = AntiAFK.Singleton.Config.GraceTime + AntiAFK.Singleton.Config.AfkTime - afkTime;
            if (gracePeriodRemaining > 0)
            {
                player.Broadcast(1, string.Format(AntiAFK.Singleton.Config.GracePeriodWarning, gracePeriodRemaining).Replace("$seconds", gracePeriodRemaining == 1 ? "second" : "seconds"), shouldClearPrevious: true);
                return;
            }

            Log.Debug($"{player} has been detected as AFK.");
            afkTime = 0;

            if (AntiAFK.Singleton.Config.TryReplace)
                TryReplace();

            player.ClearInventory();
            if (AntiAFK.Singleton.Config.SpectateLimit > 0 && ++afkCounter > AntiAFK.Singleton.Config.SpectateLimit)
            {
                player.Disconnect(AntiAFK.Singleton.Config.KickReason);
                return;
            }

            ForceSpectator();
        }

        private void TryReplace()
        {
            Player toSwap = Player.List.FirstOrDefault(x => x.Role.Type is RoleTypeId.Spectator && !x.IsOverwatchEnabled && x != player);
            if (toSwap != null)
                new PlayerInfo(player).AddTo(toSwap);
        }

        private void ForceSpectator()
        {
            player.Role.Set(RoleTypeId.Spectator);
            player.Broadcast(AntiAFK.Singleton.Config.SpectatorForced);
        }
    }
}