namespace ScuutCore.Modules.Subclasses
{
    using System.Collections.Generic;
    using PlayerRoles;
    using ScuutCore.API.Interfaces;
    using ScuutCore.Modules.Subclasses.Models;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float SpawnSubclassHintDuration { get; set; } = 5f;
        public List<SerializedSubclass> Subclasses { get; set; } = new List<SerializedSubclass>()
        {
            new SerializedSubclass()
            {
                SubclassName = "Janitor",
                SubclassSpawnChance = 15f,
                SubclassMaxAlive = 2,
                SpawnLoadout = new []
                {
                    ItemType.KeycardJanitor
                },
                RolesToReplace = new []
                {
                    RoleTypeId.ClassD
                }
            }
        };
    }
}