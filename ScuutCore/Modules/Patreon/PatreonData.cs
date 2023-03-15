namespace ScuutCore.Modules.Patreon
{
    using Types;
    using UnityEngine;
    public sealed class PatreonData : MonoBehaviour
    {

        public PatreonRank Rank { get; set; }

        public readonly CustomPatreonData Custom = new CustomPatreonData();

        public static PatreonData Get(ReferenceHub hub) => hub.TryGetComponent(out PatreonData data) ? data : hub.gameObject.AddComponent<PatreonData>();

    }
}