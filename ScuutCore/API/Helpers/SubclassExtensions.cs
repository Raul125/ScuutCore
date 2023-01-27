namespace ScuutCore.API.Helpers
{
    using System;
    using System.Collections.Generic;
    using MEC;
    using PluginAPI.Core;
    using ScuutCore.API.Features;
    using ScuutCore.Modules.Subclasses;

    public static class SubclassExtensions
    {
        public static Subclass? GetSubclass(this Player player)
        {
            var comp = player.GameObject.GetComponent<SubclassComponent>();
            if(comp == null)
                comp = player.GameObject.AddComponent<SubclassComponent>();

            return comp.CurrentSubclass;
        }
        
        public static bool TryGetSubclass(this Player player, out Subclass subclass)
        {
            return (subclass = GetSubclass(player)) == null;
        }

        public static void SetSubclass(this Player player, Subclass subclass)
        {
            var comp = player.GameObject.GetComponent<SubclassComponent>();
            if(comp == null)
                comp = player.GameObject.AddComponent<SubclassComponent>();
            comp.CurrentSubclass = subclass;
            Timing.CallDelayed(1f, () =>
            {
                player.CustomInfo = $"Subclass: {subclass.Name}";
                player.Health = subclass.Health;
                player.ClearInventory();
                player.AmmoBag.Clear();
                var itemLoadout = subclass.GetSpawnLoadout(player);
                if(itemLoadout is { Length: > 0 })
                {
                    foreach (var item in itemLoadout)
                    {
                        player.AddItem(item);
                    }
                }

                var ammoLoadout = subclass.GetAmmoLoadout(player);
                if(ammoLoadout is { Count: > 0 })
                {
                    foreach (var ammo in ammoLoadout)
                    {
                        player.SetAmmo(ammo.Key, ammo.Value);
                    }
                }
                subclass.OnReceived(player);
                Timing.CallDelayed(Subclasses.Singleton.Config.MessageDelay, () =>
                {
                    player.ReceiveHint(Subclasses.SpawnTranslations[subclass.Name],
                        Subclasses.Singleton.Config.SpawnSubclassHintDuration);
                });
            });
        }
        
        public static void RemoveSubclass(this Player player)
        {
            var comp = player.GameObject.GetComponent<SubclassComponent>();
            if(comp == null)
                comp = player.GameObject.AddComponent<SubclassComponent>();
            comp.CurrentSubclass?.OnLost(player);
            comp.CurrentSubclass = null;
            player.CustomInfo = null;
        }

        /// <summary>
        /// Adds the subclass object to a static list inside the object called _players.
        /// </summary>
        /// <param name="subclass">The subclass.</param>
        [Obsolete("feel free to remove if you read this")]
        public static void AddToList(Subclass subclass)
        {
            foreach (var field in subclass.GetType().GetFields())
            {
                if(field.Name != "_players" || field.FieldType != typeof(List<Subclass>) || !field.IsStatic)
                    continue;
                try
                {
                    ((List<Subclass>)field.GetValue(subclass)).Add(subclass);
                }
                catch (Exception e)
                {
                    Log.Error("Subclass list could not be added to: " + e);
                }
            }
        }
    }
}