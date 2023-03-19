namespace ScuutCore.API.Features
{
    using Hints;
    using MEC;
    using Modules.Subclasses;
    using PluginAPI.Core;
    using PluginAPI.Core.Interfaces;
    using UnityEngine;

    public class ScuutPlayer : Player
    {
        public ScuutPlayer(IGameComponent component) : base(component)
        {
        }

        public void SendToPocketDimension() => Position = new(0, -1999f, 0);

        public bool EnteringPocket { get; set; } = false;

        private Subclass subClass;
        private int setSubclassProc; // ensure that the delayed action does not execute if another subclass is set or destroyed

        public Subclass SubClass
        {
            get => subClass;
            set
            {
                if (value == null)
                {
                    CustomInfo = "";
                    subClass?.OnLost(this);
                    subClass = null;
                    setSubclassProc++;
                    return;
                }

                subClass = value;
                int id = ++setSubclassProc;
                Plugin.Coroutines.Add(Timing.CallDelayed(1f, () =>
                {
                    if (id != setSubclassProc)
                        return;
                    CustomInfo = $"Subclass: {SubClass.Name}";
                    Health = value.Health;
                    ClearInventory();
                    AmmoBag.Clear();
                    GrantLoadout();

                    SubClass.OnReceived(this);
                    Plugin.Coroutines.Add(Timing.CallDelayed(Subclasses.Singleton.Config.MessageDelay, () =>
                    {
                        float duration = Subclasses.Singleton.Config.SpawnSubclassHintDuration;
                        ReceiveHint(Subclasses.SpawnTranslations[SubClass.Name],
                            new HintEffect[]
                            {
                                HintEffectPresets.FadeIn(0.05f),
                                HintEffectPresets.FadeOut(0.05f, 0.95f)
                            },
                            duration);
                    }));
                }));
            }
        }

        private void GrantLoadout()
        {
            var itemLoadout = SubClass.GetSpawnLoadout(this);
            if (itemLoadout is { Length: > 0 })
            {
                foreach (var item in itemLoadout)
                    AddItem(item);
            }

            var ammoLoadout = SubClass.GetAmmoLoadout(this);
            if (SubClass.GetAmmoLoadout(this) is { Count: > 0 })
            {
                foreach (var ammo in ammoLoadout)
                    SetAmmo(ammo.Key, ammo.Value);
            }
        }

        public override void OnDestroy()
        {
        }
    }
}