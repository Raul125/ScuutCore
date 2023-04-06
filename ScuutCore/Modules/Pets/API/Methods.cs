namespace ScuutCore.Modules.Pets
{
    using Mirror;
    using PluginAPI.Core;

    public class Methods
    {
        public static void SendSpawnMessageToAll(NetworkIdentity identity)
        {
            try
            {
                foreach (Player target in Player.GetPlayers())
                    NetworkServer.SendSpawnMessage(identity, target.Connection);
            }
            catch
            {
            }
        }
    }
}