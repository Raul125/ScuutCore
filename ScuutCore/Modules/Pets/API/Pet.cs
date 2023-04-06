namespace ScuutCore.Modules.Pets
{
    using Mirror;
    using PlayerRoles;
    using PlayerRoles.FirstPersonControl;
    using PluginAPI.Core;
    using ScuutCore.Modules.Patreon;
    using ScuutCore.Modules.ScpSwap.Commands;
    using System.Collections.Generic;
    using UnityEngine;

    public class Pet
    {
        public static List<Pet> List = new();
        public static Pet Create(Player owner)
        {
            Pet pet = new()
            {
                Owner = owner,
                ReferenceHub = Object.Instantiate(NetworkManager.singleton.playerPrefab).GetComponent<ReferenceHub>()
            };

            pet.Player = Player.Get(pet.ReferenceHub);

            NetworkServer.AddPlayerForConnection(new FakeConnection(), pet.ReferenceHub.gameObject);

            PetData petData;
            if (!owner.ReferenceHub.TryGetComponent(out PatreonData data))
            {
                petData = new()
                {
                    Name = $"{owner.Nickname}'s Pet"
                };
            }

            petData = data.Prefs.PetData;

            pet.ReferenceHub.nicknameSync.Network_myNickSync = petData.Name;
            pet.ReferenceHub.characterClassManager._privUserId = "NPC";

            pet.ReferenceHub.roleManager.ServerSetRole(petData.RoleType, RoleChangeReason.RemoteAdmin);

            pet.ReferenceHub.TryOverridePosition(owner.Position, owner.Rotation);
            pet.ReferenceHub.characterClassManager.GodMode = true;

            pet.ReferenceHub.transform.parent = owner.GameObject.transform;
            pet.ReferenceHub.transform.localPosition = new Vector3(0, -0.3f, 0);

            pet.ReferenceHub.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
            Methods.SendSpawnMessageToAll(pet.ReferenceHub.networkIdentity);

            var item = pet.Player.AddItem(petData.ItemType);
            pet.Player.CurrentItem = item;

            List.Add(pet);
            return pet;
        }

        public void RefreshData(PetData petData)
        {
            if (petData.Name != ReferenceHub.nicknameSync.Network_myNickSync)
                ReferenceHub.nicknameSync.Network_myNickSync = petData.Name;

            if (Player.Role != petData.RoleType)
                Player.Role = petData.RoleType;

            if (petData.ItemType != Player.CurrentItem?.ItemTypeId)
            {
                Player.ClearInventory();
                var item = Player.AddItem(petData.ItemType);
                Player.CurrentItem = item;
            }
        }

        public Player Owner { get; private set; }
        public Player Player { get; private set; }
        public ReferenceHub ReferenceHub { get; private set; }

        public void Destroy() => Player.Disconnect();
    }
}