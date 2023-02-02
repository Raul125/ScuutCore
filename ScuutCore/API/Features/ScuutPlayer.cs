namespace ScuutCore.API.Features
{
    using MEC;
    using PluginAPI.Core;
    using PluginAPI.Core.Interfaces;
    using ScuutCore.Modules.Subclasses;

    public class ScuutPlayer : Player
    {
        public ScuutPlayer(IGameComponent component) : base(component)
        {
        }

        public Subclass SubClass 
        {
            get => SubClass;
            set
            {
                if (value == null)
                {
                    SubClass?.OnLost(this);
                    SubClass = null;
                    this.CustomInfo = null;
                    return;
                }

                SubClass = value;
                Plugin.Coroutines.Add(Timing.CallDelayed(1f, () =>
                {
                    this.CustomInfo = $"Subclass: {SubClass.Name}";
                    this.Health = value.Health;
                    this.ClearInventory();
                    this.AmmoBag.Clear();
                    var itemLoadout = SubClass.GetSpawnLoadout(this);
                    if (itemLoadout is { Length: > 0 })
                    {
                        foreach (var item in itemLoadout)
                            this.AddItem(item);
                    }

                    var ammoLoadout = SubClass.GetAmmoLoadout(this);
                    if (ammoLoadout is { Count: > 0 })
                    {
                        foreach (var ammo in ammoLoadout)
                            this.SetAmmo(ammo.Key, ammo.Value);
                    }

                    SubClass.OnReceived(this);
                    Plugin.Coroutines.Add(Timing.CallDelayed(Subclasses.Singleton.Config.MessageDelay, () =>
                    {
                        this.ReceiveHint(Subclasses.SpawnTranslations[SubClass.Name],
                            Subclasses.Singleton.Config.SpawnSubclassHintDuration);
                    }));
                }));
            }
        }

        public override void OnDestroy()
        {
        }
    }
}