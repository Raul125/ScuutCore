namespace ScuutCore.API.Extensions;

using PlayerRoles;
using UnityEngine;

public static class Extensions
{
    public static Color GetColor(this RoleTypeId roleType) => roleType == RoleTypeId.None ? Color.white : roleType.GetRoleBase().RoleColor;

    public static PlayerRoleBase GetRoleBase(this RoleTypeId roleType) => roleType.TryGetRoleBase(out PlayerRoleBase roleBase) ? roleBase : null;

    public static bool TryGetRoleBase(this RoleTypeId roleType, out PlayerRoleBase roleBase) => PlayerRoleLoader.TryGetRoleTemplate(roleType, out roleBase);

    /// <summary>
    /// Removes the prefab-generated brackets (#) on <see cref="UnityEngine.GameObject"/> names.
    /// </summary>
    /// <param name="name">Name of the <see cref="UnityEngine.GameObject"/>.</param>
    /// <returns>Name without brackets.</returns>
    public static string RemoveBracketsOnEndOfName(this string name)
    {
        int bracketStart = name.IndexOf('(') - 1;

        if (bracketStart > 0)
            name = name.Remove(bracketStart, name.Length - bracketStart);

        return name;
    }
}