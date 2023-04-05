namespace ScuutCore.Modules.Pets
{
    using Mirror;
    using PlayerRoles;
    using PluginAPI.Core;
    using System;
    using System.Collections.Generic;

    public class Pet
    {
        public static List<Pet> List = new();
        private static int _connectionId = 1000;
        public static Pet Create(Player owner)
        {
            Pet pet = new();
            pet.Owner = owner;
            var newPlayer = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var fakeConnection = new FakeConnection(_connectionId);
            _connectionId++;

            pet.ReferenceHub = newPlayer.GetComponent<ReferenceHub>();
            pet.Player = Player.Get(pet.ReferenceHub);

            NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);

            try
            {
                pet.ReferenceHub.characterClassManager.UserId = $"npc";
            }
            catch (Exception)
            {
                //Ignore
            }

            try
            {
                pet.ReferenceHub.nicknameSync.SetNick("Petty");
            }
            catch (Exception)
            {
                //Ignore
            }

            pet.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);

            return pet;
        }

        public Player Owner { get; private set; }
        public Player Player { get; private set; }
        public ReferenceHub ReferenceHub { get; private set; }
    }
}