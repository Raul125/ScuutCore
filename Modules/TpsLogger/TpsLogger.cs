using MEC;
using ScuutCore.API;
using System.Collections.Generic;

namespace ScuutCore.Modules.TpsLogger
{
    public class TpsLogger : Module<Config>
    {
        public override string Name { get; } = "TpsLogger";
        public WebhookController WebhookController { get; private set; }
        private CoroutineHandle webhookCoroutine;

        public override void OnEnabled()
        {
            if (Config.EnableWebhook && !string.IsNullOrEmpty(Config.Url))
            {
                WebhookController = new WebhookController(this);
                webhookCoroutine = Timing.RunCoroutine(RunWebhook());
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            if (webhookCoroutine.IsRunning)
                Timing.KillCoroutines(webhookCoroutine);

            base.OnDisabled();
        }

        private IEnumerator<float> RunWebhook()
        {
            while (true)
            {
                if (!Config.LogIdle)
                    yield return Timing.WaitUntilTrue(() => !IdleMode.IdleModeActive);

                yield return Timing.WaitForSeconds(Config.Interval);
                WebhookController.SendTps();
            }
        }
    }
}