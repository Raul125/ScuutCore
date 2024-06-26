﻿namespace ScuutCore.Modules.AutoNuke;

using System;
using System.Collections.Generic;
using ScuutCore.API.Features;
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

public sealed class EventHandlers : InstanceBasedEventHandler<AutoNuke>
{
    public bool IsAutoNuke;
    public DateTime WarheadTime { get; set; }

    [PluginEvent(ServerEventType.RoundStart)]
    public void OnRoundStart()
    {
        WarheadTime = DateTime.Now.AddSeconds(Module.Config.AutoNukeStartTime);
        IsAutoNuke = false;
        Plugin.Coroutines.Add(Timing.RunCoroutine(AutoNukeCoroutine()));
    }

    [PluginEvent(ServerEventType.WarheadStop)]
    public bool OnWarheadStopping(Player player)
    {
        if (!IsAutoNuke)
            return true;
        Module.Config.CantDisableHint.Show(player);
        return false;
    }

    public IEnumerator<float> AutoNukeCoroutine()
    {
        yield return Timing.WaitForSeconds(Module.Config.AutoNukeWarn);

        if (Warhead.IsDetonated)
            yield break;

        Module.Config.AutoNukeCassieWarn.Play();
        Module.Config.AutoNukeWarnBroadcast.ShowAll();

        Module.Config.AutoNukeWarnHint.ShowAll();
        yield return Timing.WaitForSeconds(Module.Config.AutoNukeStartTime - Module.Config.AutoNukeWarn);

        IsAutoNuke = true;
        if (Warhead.IsDetonated || Warhead.IsDetonationInProgress)
            yield break;
        Module.Config.AutoNukeCassieStart.Play();
        Module.Config.AutoNukeStartBroadcast.ShowAll();

        Module.Config.AutoNukeStartHint.ShowAll();
        AlphaWarheadController.Singleton.IsLocked = false;
        AlphaWarheadController.Singleton.CooldownEndTime = 0;
        AlphaWarheadController.Singleton.StartDetonation(Module.Config.IsVanillaAutomaticNuke);
    }
}