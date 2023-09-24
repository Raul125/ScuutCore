namespace ScuutCore.API.Features;

using Hints;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using Modules.Subclasses;
using PlayerRoles.Voice;
using PluginAPI.Core;
using PluginAPI.Core.Interfaces;
using UnityEngine;

public sealed class ScuutPlayer : Player
{
    public ScuutPlayer(IGameComponent component) : base(component)
    {
    }

    public void SendToPocketDimension() => Position = new(0, -1999f, 0);

    public bool EnteringPocket { get; set; } = false;

    private Subclass? subClass;
    private int setSubclassProc; // ensure that the delayed action does not execute if another subclass is set or destroyed

    public Subclass? SubClass
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
            {
                var itemObj = AddItem(item);
                if (itemObj is Firearm firearm)
                {
                    if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(ReferenceHub, out var dictionary) && dictionary.TryGetValue(itemObj.ItemTypeId, out var code))
                    {
                        firearm.ApplyAttachmentsCode(code, true);
                        if (Subclasses.Singleton?.Config.LoadGuns ?? false)
                        {
                            firearm.Status = new FirearmStatus(firearm.AmmoManagerModule.MaxAmmo, FirearmStatusFlags.Chambered | FirearmStatusFlags.Cocked | FirearmStatusFlags.MagazineInserted, firearm.GetCurrentAttachmentsCode());
                        }
                        else
                            firearm.Status = new FirearmStatus(0, firearm.Status.Flags, firearm.GetCurrentAttachmentsCode());
                    }
                }
            }
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
        base.OnDestroy();
    }

    public float SpeechUpdateTime;

    public override void OnUpdate()
    {
        SpeechUpdateTime += Time.deltaTime;
        if (SpeechUpdateTime < 0.5f)
            return;
        SpeechUpdateTime = 0;
        if (RoleBase is not IVoiceRole vcRole || !vcRole.VoiceModule.ServerIsSending || !Modules.ScpSpeech.EventHandlers.ScpsToggled.Contains(ReferenceHub))
            return;
        ReceiveHint("\n\n\n\n\n\n\n\n\n\n\n\n<size=18>You are using Proximity Chat\nPress <mark=#adadad33>Left Alt</mark> to switch to SCP Chat</size>", 0.6f);
        base.OnUpdate();
    }
}