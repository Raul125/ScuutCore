namespace ScuutCore.Modules.Pets
{
    using Mirror;
    using System;

    public class FakeConnection : NetworkConnectionToClient
    {
        private static int _idGenerator = int.MaxValue;

        public FakeConnection()
            : base(_idGenerator--, true, 0)
        {
        }

        public override string address => "npc";

        public override void Send(ArraySegment<byte> segment, int channelId = 0)
        {
        }

        public override void Disconnect()
        {
            Pet.List.RemoveAll(x => x.ReferenceHub == identity.gameObject.GetComponent<ReferenceHub>());
            NetworkServer.RemovePlayerForConnection(this, true);
        }
    }
}
