using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Items.Usables.Scp330;
using MEC;
using UnityEngine;

namespace ScuutCore.Modules.AntiAFK
{
    public class PlayerInfo
    {
        private readonly List<ItemType> items = new List<ItemType>();
        private readonly List<CandyKindID> candies = new List<CandyKindID>();
        private readonly List<CustomItem> customItems = new List<CustomItem>();
        private readonly RoleType role;
        private readonly Vector3 position;
        private readonly float health;
        private readonly IReadOnlyCollection<CustomRole> customRoles;
        private readonly byte level;
        private readonly float experience;
        private readonly float energy;

        public PlayerInfo(Player player)
        {
            role = player.Role.Type;
            position = player.Position;
            health = player.Health;
            CustomRole.TryGet(player, out customRoles);

            foreach (Item item in player.Items)
            {
                if (CustomItem.TryGet(item, out CustomItem customItem))
                    customItems.Add(customItem);
                else if (item is Scp330 scp330)
                    candies.AddRange(scp330.Candies);
                else
                    items.Add(item.Type);
            }

            if (player.Role is Scp079Role scp079Role)
            {
                level = scp079Role.Level;
                experience = scp079Role.Experience;
                energy = scp079Role.Energy;
            }
        }

        public void AddTo(Player player)
        {
            player.Role.Type = role;

            bool isCustom = customRoles != null;
            if (isCustom)
            {
                foreach (CustomRole customRole in customRoles)
                    customRole.AddRole(player);
            }

            Timing.CallDelayed(2f, () =>
            {
                player.Health = health;
                player.Position = position;
                player.ResetInventory(items);

                foreach (CustomItem customItem in customItems)
                    customItem.Give(player);

                if (candies.Count > 0)
                {
                    Scp330 scp330 = (Scp330)Item.Create(ItemType.SCP330);
                    foreach (CandyKindID candy in candies)
                        scp330.AddCandy(candy);

                    scp330.Give(player);
                }

                if (player.Role is Scp079Role scp079Role)
                {
                    scp079Role.Level = level;
                    scp079Role.Experience = experience;
                    scp079Role.Energy = energy;
                    scp079Role.MaxEnergy = scp079Role.Levels[level].maxMana;
                }
            });
        }
    }
}