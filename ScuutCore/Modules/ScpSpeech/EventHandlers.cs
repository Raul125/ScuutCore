using Exiled.Events.EventArgs;

namespace ScuutCore.Modules.ScpSpeech
{
    public class EventHandlers
    {
        private ScpSpeech scpSpeech;
        public EventHandlers(ScpSpeech scpsp)
        {
            scpSpeech = scpsp;
        }

        public void OnTransmitting(TransmittingEventArgs ev)
        {
            if (scpSpeech.Config.GlobalTalking.Contains(ev.Player.Role))
            {
                ev.Player.DissonanceUserSetup.MimicAs939 = ev.IsTransmitting;
            }
        }
    }
}