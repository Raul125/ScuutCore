namespace ScuutCore.Modules.AntiAFK.Models
{
    using System.Collections.Generic;
    using InventorySystem.Items.Usables.Scp330;
    using MEC;
    using PlayerRoles;
    using PlayerRoles.PlayableScps.Scp079;
    using PluginAPI.Core;
    using UnityEngine;

    public class PlayerInfo
    {
        private readonly List<ItemType> items = new List<ItemType>();
        private readonly List<CandyKindID> candies = new List<CandyKindID>();
        private readonly RoleTypeId role;
        private readonly Vector3 position;
        private readonly float health;
        private readonly int level;
        private readonly int experience;
        private readonly float energy;

        public PlayerInfo(Player player)
        {
            role = player.Role;
            position = player.Position;
            health = player.Health;

            foreach (var item in player.Items)
                items.Add(item.ItemTypeId);

            if (player.ReferenceHub.roleManager.CurrentRole is Scp079Role scp079)
            {
                bool sub = scp079.SubroutineModule.TryGetSubroutine(out Scp079TierManager ability);
                level = sub ? ability.AccessTierLevel : 0; ;
                experience = sub ? ability.TotalExp : 0;
                energy = scp079.SubroutineModule.TryGetSubroutine(out Scp079AuxManager abilityaux) ? abilityaux.CurrentAux : 0;
            }
        }

        public void AddTo(Player player)
        {
            player.SetRole(role);

            Timing.CallDelayed(3f, () =>
            {
                player.Health = health;
                player.Position = position;

                foreach (var item in items)
                    player.AddItem(item);

                if (player.ReferenceHub.roleManager.CurrentRole is Scp079Role scp079
                    && scp079.SubroutineModule.TryGetSubroutine(out Scp079TierManager ability))
                {
                    ability.TotalExp = level <= 1 ? 0 : ability.AbsoluteThresholds[Mathf.Clamp(level - 2, 0, ability.AbsoluteThresholds.Length - 1)];
                    ability.TotalExp = experience;

                    if (!scp079.SubroutineModule.TryGetSubroutine(out Scp079AuxManager abilityaux))
                        return;

                    abilityaux.CurrentAux = energy;
                }
            });
        }
    }
}