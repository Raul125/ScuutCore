namespace ScuutCore.Modules.Patreon.Types;

using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

[Serializable]
public struct PatreonRank
{
    public string Id { get; set; }
    public int Priority { get; set; }

    public List<Badge> BadgeOptions { get; set; }

    public string DefaultCustomColor { get; set; }

    [YamlIgnore]
    public bool IsValid => !string.IsNullOrEmpty(Id) && BadgeOptions != null;

}