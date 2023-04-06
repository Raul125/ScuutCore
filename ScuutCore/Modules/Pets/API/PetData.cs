namespace ScuutCore.Modules.Pets
{
    using PlayerRoles;
    using System;

    [Serializable]
    public class PetData
    {
        public string Name { get; set; } = string.Empty;

        public RoleTypeId RoleType { get; set; } = RoleTypeId.ClassD;

        public ItemType ItemType { get; set; } = ItemType.None;

        public bool IsHidden { get; set; } = false;
    }
}
