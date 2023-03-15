namespace ScuutCore.Modules.Patreon.Types
{
    using System;
    [Serializable]
    public struct UserRankAssociation
    {

        public string UserId { get; set; }

        public string RankId { get; set; }

    }
}