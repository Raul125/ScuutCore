namespace ScuutCore.Modules.ScpSpeech
{
    using API.Features;
    public sealed class ScpSpeechModule : Module<Config>
    {

        public static ScpSpeechModule Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Instance = null;
            base.OnDisabled();
        }

    }
}