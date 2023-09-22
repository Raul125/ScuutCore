namespace ScuutCore.API.Helpers
{
    using System.Collections.Generic;
    using NWAPIPermissionSystem;
    using PluginAPI.Core;

    public static class GlobalHelpers
    {
        public static void BroadcastToPermissions(string message, string permission, ushort duration = 5)
        {
            foreach (var player in Player.GetPlayers())
            {
                if (player.CheckPermission(permission))
                {
                    player.SendBroadcast(message, duration);
                }
            }
        }
    }
}