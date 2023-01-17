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
            comp ??= player.GameObject.AddComponent<SubclassComponent>();
            return comp.CurrentSubclass;
        }
        
        public static bool TryGetSubclass(this Player player, out Subclass subclass)
        {
            return (subclass = GetSubclass(player)) == null;
        }
        
        public static void SetSubclass(this Player player, Subclass subclass)
        {
            var comp = player.GameObject.GetComponent<SubclassComponent>();
            comp ??= player.GameObject.AddComponent<SubclassComponent>();
            comp.CurrentSubclass = subclass;
            Timing.CallDelayed(1f, () =>
            {
                foreach (var item in subclass.GetSpawnLoadout(player)!)
                {
                    player.AddItem(item);
                }
                subclass.OnReceived(player);
                player.ReceiveHint(Subclasses.SpawnTranslations[subclass.Name], Subclasses.Singleton.Config.SpawnSubclassHintDuration);
            });
        }
        
        public static void RemoveSubclass(this Player player)
        {
            var comp = player.GameObject.GetComponent<SubclassComponent>();
            comp ??= player.GameObject.AddComponent<SubclassComponent>();
            comp.CurrentSubclass?.OnLost(player);
            comp.CurrentSubclass = null;
        }
    }
}