namespace ScuutCore.API.Helpers
{
    using InventorySystem.Items.Usables.Scp330;
    using PluginAPI.Core;
    using Utils.Networking;

    public static class PlayerDeathEffects
    {
        public static void PlayExplosionEffect(Player ply)
        {
            new CandyPink.CandyExplosionMessage
            {
                Origin = ply.Position
            }.SendToAuthenticated();
        }
    }
}