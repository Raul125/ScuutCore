namespace ScuutCore.Modules.Restart
{
    using GameCore;
    using MEC;
    using PluginAPI.Core;
    using UnityEngine;

    public class EventHandlers
    {
        private Restart restart;
        public EventHandlers(Restart rs)
        {
            restart = rs;
        }


        public int Rounds = 0;
        public void OnRoundRestarting()
        {
            Rounds++;
        }

        public void OnRoundEnd()
        {
            if (Rounds == restart.Config.RestartAfterRounds)
            {
                float time = Mathf.Clamp(ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000) - 0.5f;
                Timing.CallDelayed(time, () => Server.Restart());
            }
        }
    }
}