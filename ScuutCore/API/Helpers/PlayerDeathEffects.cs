namespace ScuutCore.API.Helpers
{
    using PluginAPI.Core;
    using Utils;

    public static class PlayerDeathEffects
    {
        public static void PlayExplosionEffect(Player ply)
        {
            ExplosionUtils.ServerSpawnEffect(ply.Position,ItemType.GrenadeHE);
        }
    }
}