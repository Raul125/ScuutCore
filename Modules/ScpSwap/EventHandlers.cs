using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace ScuutCore.Modules.ScpSwap
{
    public class EventHandlers
    {
        private ScpSwap scpSwap;
        public EventHandlers(ScpSwap sc)
        {
            scpSwap = sc;
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsScp || ValidSwaps.GetCustom(ev.Player) != null)
                return;

            Timing.CallDelayed(0.1f, () =>
            {
                if ((ev.Player.IsScp || ValidSwaps.GetCustom(ev.Player) != null) &&
                    Round.ElapsedTime.TotalSeconds < scpSwap.Config.SwapTimeout)
                    ev.Player.Broadcast(scpSwap.Config.StartMessage);
            });
        }

        public void OnRestartingRound()
        {
            Swap.Clear();
        }

        public void OnWaitingForPlayers()
        {
            ValidSwaps.Refresh();
        }
    }
}