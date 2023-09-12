namespace ScuutCore.Modules.AntiAFK.Components
{
    using System.Linq;
    using PlayerRoles;
    using PluginAPI.Core;
    using ScuutCore.Modules.AntiAFK.Models;
    using ScuutCore.Modules.Patreon;
    using UnityEngine;
    using PlayerInfo = ScuutCore.Modules.AntiAFK.Models.PlayerInfo;

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
            if (player.Role is RoleTypeId.Scp079 ||
                !player.IsAlive ||
                AntiAFK.Singleton.Config.MinimumPlayers > Player.Count ||
                (AntiAFK.Singleton.Config.IgnoreTutorials && player.Role == RoleTypeId.Tutorial))
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
                player.SendBroadcast(string.Format(AntiAFK.Singleton.Config.GracePeriodWarning, gracePeriodRemaining).Replace("$seconds", gracePeriodRemaining == 1 ? "second" : "seconds"), 1, shouldClearPrevious: true);
                return;
            }

            Log.Debug($"{player.Nickname} has been detected as AFK.");
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
            var toSwap = Player.GetPlayers()
                .OrderByDescending(x => x.GetPriorityPatreon())
                .FirstOrDefault(x => x.Role is RoleTypeId.Spectator && !x.IsOverwatchEnabled && x != player);
            if (toSwap != null)
                new PlayerInfo(player).AddTo(toSwap);
        }

        private void ForceSpectator()
        {
            player.SetRole(RoleTypeId.Spectator);
            AntiAFK.Singleton.Config.SpectatorForced.Show(player);
        }
    }
}