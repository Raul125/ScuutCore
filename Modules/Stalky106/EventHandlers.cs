using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using NorthwoodLib.Pools;

namespace ScuutCore.Modules.Stalky106
{
    public class EventHandlers
    {
        private Stalky106 stalky106;
        public EventHandlers(Stalky106 st)
        {
            stalky106 = st;
        }

		public void OnRoundStart()
		{
			stalky106.Methods.StalkyCooldown = stalky106.Config.InitialCooldown;
		}

		public void OnChangingRole(ChangingRoleEventArgs ev)
		{
			if (ev.NewRole == RoleType.Scp106)
			{
				Plugin.Coroutines.Add(Timing.CallDelayed(0.5f, () =>
				{
					if (ev.Player.Role == RoleType.Scp106)
					{
						ev.Player.Broadcast(10, stalky106.Config.WelcomeBroadcast);
						ev.Player.SendConsoleMessage(stalky106.Config.ConsoleInfo, "white");
					}
				}));
			}
		}

		public void OnCreatePortal(CreatingPortalEventArgs ev)
		{
			if (Warhead.IsDetonated && stalky106.Config.DisableAutoNuke && AutoNuke.EventHandlers.IsAutoNuke)
            {
				ev.Player.ShowHint(stalky106.Config.DisableAutoNukeHint.Message, stalky106.Config.DisableAutoNukeHint.Time);
				return;
            }

			var list = ListPool<Player>.Shared.Rent(Player.List);
			int playerCount = list.Count;
			if (stalky106.Config.MinimumPlayers > playerCount)
			{
				ev.Player.Broadcast(8, stalky106.Config.MinPlayers.Replace("$count", stalky106.Config.MinimumPlayers.ToString()));
				stalky106.Methods.StalkyCooldown = 10f;
				return;
			}

			int aliveCount = 0;
			int targetCount = 0;
			for (int i = 0; i < playerCount; i++)
			{
				var ply = list[i];
				// if it's a target
				if (ply.IsAlive)
				{
					aliveCount++;
					if (!stalky106.Config.IgnoreRoles.Contains(ply.Role) && !stalky106.Config.IgnoreTeams.Contains(ply.Role.Team))
					{
						targetCount++;
					}
				}
			}

			if (stalky106.Config.MinimumAlivePlayers > aliveCount)
			{
				ev.Player.Broadcast(8, stalky106.Config.MinAlive.Replace("$count", stalky106.Config.MinimumAlivePlayers.ToString()));
				stalky106.Methods.StalkyCooldown = 10f;
				return;
			}

			if (stalky106.Config.MinimumAliveTargets > targetCount)
			{
				ev.Player.Broadcast(8, stalky106.Config.MinTargetsAlive.Replace("$count", stalky106.Config.MinimumAliveTargets.ToString()));
				stalky106.Methods.StalkyCooldown = 10f;
				return;
			}

			ListPool<Player>.Shared.Return(list);
			ev.IsAllowed = stalky106.Methods.Stalk(ev.Player);
		}
	}
}