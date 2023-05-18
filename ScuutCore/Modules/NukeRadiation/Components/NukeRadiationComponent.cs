namespace ScuutCore.Modules.NukeRadiation.Components
{
    using CustomPlayerEffects;
    using PluginAPI.Core;
    using UnityEngine;

    public class NukeRadiationComponent : MonoBehaviour
    {
        private Player player;
        private float Time = 0f;
        private float MessageTimeLeft = 0f;
        private float DamageTimeLeft = 0f;
        public bool IsInNukeZone = false;
        private void Start()
        {
            player = Player.Get(gameObject);
        }

        private void FixedUpdate()
        {
            if (player.Position.y is < -700 and > -720 && player.IsAlive)
            {
                bool hasEffect = player.EffectsManager.GetEffect<Decontaminating>().IsEnabled;
                Time += UnityEngine.Time.fixedDeltaTime;
                bool dealDamage = NukeRadiation.Singleton!.Config.RadiationTime - Time <= 0f;
                if (DamageTimeLeft >= 0f)
                {
                    DamageTimeLeft -= UnityEngine.Time.fixedDeltaTime;
                }
                else
                {
                    if (dealDamage)
                    {
                        if (!hasEffect)
                            player.EffectsManager.EnableEffect<Decontaminating>();
                        player.Damage(player.MaxHealth*(NukeRadiation.Singleton.Config.RadiationDamagePercent/100), "Radiation");
                        DamageTimeLeft = NukeRadiation.Singleton.Config.RadiationInterval;
                    }
                }
                if (MessageTimeLeft >= 0f)
                {
                    MessageTimeLeft -= UnityEngine.Time.fixedDeltaTime;
                }
                else
                {
                    if (!IsInNukeZone)
                    {
                        Log.Info("Entered");
                        player.ReceiveHint(NukeRadiation.Singleton.Config.EnterRadiationMessage, 5f);
                        MessageTimeLeft = 5f;
                    }
                    else if (dealDamage)
                    {
                        player.ReceiveHint(NukeRadiation.Singleton.Config.RadiationMessage, 1f);
                        MessageTimeLeft = 1f;
                    }
                    else if (NukeRadiation.Singleton.Config.EnableTimeLeftCountdown)
                    {
                        var diff = (int)(NukeRadiation.Singleton.Config.RadiationTime - Time);
                        player.ReceiveHint(NukeRadiation.Singleton.Config.TimeLeftCountdownMessage.Replace("%time%",
                            (diff < 0 ? 0 : diff).ToString()), 1);
                        MessageTimeLeft = 1f;
                    }
                }
                IsInNukeZone = true;
            }
            else if (Time > 0f)
            {
                Time = 0f;
                IsInNukeZone = false;
                player.EffectsManager.DisableEffect<Decontaminating>();
            }
        }
    }
}