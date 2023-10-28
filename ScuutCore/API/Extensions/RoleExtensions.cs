namespace ScuutCore.API.Extensions;

using PlayerRoles;
using UnityEngine;

public static class RoleExtensions
{
    public static Color GetColor(this RoleTypeId roleType) => roleType == RoleTypeId.None ? Color.white : roleType.GetRoleBase().RoleColor;

    public static PlayerRoleBase GetRoleBase(this RoleTypeId roleType) => roleType.TryGetRoleBase(out PlayerRoleBase roleBase) ? roleBase : null;

    public static bool TryGetRoleBase(this RoleTypeId roleType, out PlayerRoleBase roleBase) => PlayerRoleLoader.TryGetRoleTemplate(roleType, out roleBase);

    public static void HideTag(this ServerRoles serverRoles)
    {
        serverRoles.GlobalHidden = serverRoles.GlobalSet;
        serverRoles.HiddenBadge = serverRoles.MyText;
        serverRoles.NetworkGlobalBadge = null;
        serverRoles.SetText(null);
        serverRoles.SetColor(null);
        serverRoles.RefreshHiddenTag();
    }
}