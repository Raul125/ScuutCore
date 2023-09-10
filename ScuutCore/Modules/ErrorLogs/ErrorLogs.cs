namespace ScuutCore.Modules.ErrorLogs
{
    using API.Features;
    using MEC;

    public sealed class ErrorLogs : SingletonControllerModule<ErrorLogs, Config>
    {
        private CoroutineHandle _coroutineHandle;

        public override void OnEnabled()
        {
            _coroutineHandle = Timing.RunCoroutine(WebhookSender.ManageQueue());
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Timing.KillCoroutines(_coroutineHandle);
            base.OnDisabled();
        }
    }
}