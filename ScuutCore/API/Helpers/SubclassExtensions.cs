namespace ScuutCore.API.Helpers
{
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
                player.ReceiveHint(Subclasses.SpawnTranslations[subclass.Name], Subclasses.Singleton.Config.SpawnSubclassHintDuration);
            });
        }
        
        public static void RemoveSubclass(this Player player)
        {
            var comp = player.GameObject.GetComponent<SubclassComponent>();
            if(comp == null)
                comp = player.GameObject.AddComponent<SubclassComponent>();
            comp.CurrentSubclass?.OnLost(player);
            comp.CurrentSubclass = null;
        }
    }
}