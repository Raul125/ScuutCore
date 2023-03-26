namespace ScuutCore
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using PlayerRoles;
    using PluginAPI.Helpers;

    public class Config
    {
        public string ConfigsFolder = Path.Combine(Paths.GlobalPlugins.Plugins, "ScuutCore");
        public bool Debug { get; set; } = false;

        public Dictionary<string, ushort> ServerCommand { get; set; } = new Dictionary<string, ushort>()
        {
            {
                "sv1", 25570
            },
            {
                "sv2", 25571
            }
        };

        public List<RoleTypeId> SuicideDisabledRoles { get; set; } = new List<RoleTypeId>()
        {
            RoleTypeId.Scp106,
            RoleTypeId.Scp049,
            RoleTypeId.Scp096,
            RoleTypeId.Scp173,
            RoleTypeId.Scp0492,
            RoleTypeId.Scp079,
            RoleTypeId.Scp939
        };

        [Description("Percentage change of spawning a 1576 instead of a 2176 in the pedestal (max 1 per round).")]
        public float Scp1576SpawnChance { get; set; } = 50f;
    }
}