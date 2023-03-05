namespace ScuutCore.Modules.ErrorLogs
{
    using API.Features;
    using MEC;

    public sealed class ErrorLogs : Module<Config>
    {
        public static ErrorLogs Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            Timing.RunCoroutine(WebhookSender.ManageQueue());
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            base.OnDisabled();
        }
    }
}