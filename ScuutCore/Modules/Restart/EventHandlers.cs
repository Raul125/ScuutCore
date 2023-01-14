namespace ScuutCore.Modules.Restart
{
    using GameCore;
    using MEC;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;
    using RoundSummary = global::RoundSummary;

    public class EventHandlers
    {
        private Restart restart;
        public EventHandlers(Restart rs)
        {
            restart = rs;
        }

        public int Rounds = 0;

        [PluginEvent(ServerEventType.RoundRestart)]
        public void OnRoundRestarting()
        {
            Rounds++;
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(RoundSummary.LeadingTeam leadingTeam)
        {
            if (Rounds == restart.Config.RestartAfterRounds)
            {
                float time = Mathf.Clamp(ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000) - 0.5f;
                Timing.CallDelayed(time, () => Server.Restart());
            }
        }
    }
}