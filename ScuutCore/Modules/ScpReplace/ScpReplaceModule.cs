namespace ScuutCore.Modules.ScpReplace
{
    using System.Collections.Generic;
    using ScuutCore.API.Features;
    using ScuutCore.Modules.ScpReplace.Models;

    public class ScpReplaceModule : EventControllerModule<ScpReplaceModule, ScpReplaceConfig, EventHandler>
    {
        public static List<ReplaceInfo> ReplaceInfos { get; } = new List<ReplaceInfo>();
    }
}