namespace ScuutCore.Modules.RoundSummary
{
    using PlayerStatsSystem;
    using ScuutCore.API.Features;

    public sealed class RoundSummary : EventControllerModule<RoundSummary, Config, EventHandlers>
    {
        public override void OnEnabled()
        {
            PlayerStats.OnAnyPlayerDamaged += EventHandlers.OnAnyDamage;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerStats.OnAnyPlayerDamaged -= EventHandlers.OnAnyDamage;
            base.OnDisabled();
        }
    }
}