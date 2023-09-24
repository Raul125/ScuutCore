namespace ScuutCore.Modules.NukeRadiation;

using ScuutCore.API.Features;
using ScuutCore.Modules.NukeRadiation.Components;
using UnityEngine;

public sealed class NukeRadiation : EventControllerModule<NukeRadiation, Config, EventHandlers>
{
    public override void OnDisabled()
    {
        foreach (var hub in ReferenceHub.AllHubs)
        {
            Object.Destroy(hub.gameObject.GetComponent<NukeRadiationComponent>());
        }

        base.OnDisabled();
    }
}