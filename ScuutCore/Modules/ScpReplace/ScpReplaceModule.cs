namespace ScuutCore.Modules.ScpReplace;

using System.Collections.Generic;
using PluginAPI.Core;
using ScuutCore.API.Features;
using ScuutCore.Modules.ScpReplace.Models;

public class ScpReplaceModule : EventControllerModule<ScpReplaceModule, ScpReplaceConfig, EventHandler>
{
    public static List<ReplaceInfo> ReplaceInfos { get; } = new List<ReplaceInfo>();

    public void Debug(string message)
    {
        if (Config.Debug)
            Log.Debug(message);
    }
}