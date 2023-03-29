namespace ScuutCore.Modules.NukeRadiation.Components
{
    using PluginAPI.Core;
    using UnityEngine;

    public class NukeRadiationComponent : MonoBehaviour
    {
        private Player player;
        private float Time = 0f;
        private float MessageTimeLeft = 0f;
        private float DamageTimeLeft = 0f;
        private bool IsInNukeZone = false;
        private void Start()
        {
            player = Player.Get(gameObject);
        }

        private void FixedUpdate()
        {
            if (player.Position.y is < -700 and > -720)
            {
                Time += UnityEngine.Time.fixedDeltaTime;
                bool dealDamage = NukeRadiation.Singleton.Config.RadiationTime - Time <= 0f;
                if (DamageTimeLeft >= 0f)
                {
                    DamageTimeLeft -= UnityEngine.Time.fixedDeltaTime;
                }
                else
                {
                    if (dealDamage)
                    {
                        player.Damage(NukeRadiation.Singleton.Config.RadiationDamage, "Radiation");
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
                        player.ReceiveHint(NukeRadiation.Singleton.Config.TimeLeftCountdownMessage.Replace("%time%",
                            ((int)Time).ToString()), 1);
                        MessageTimeLeft = 1f;
                    }
                }
                IsInNukeZone = true;
            }
            else if (Time > 0f)
            {
                Time = 0f;
                IsInNukeZone = false;
            }
        }
    }
}