namespace ScuutCore.Modules.Patreon
{
    using System.Collections.Generic;
    using Types;
    using UnityEngine;

    public sealed class PatreonData : MonoBehaviour
    {

        private static readonly Dictionary<string, CustomPatreonData> CustomData = new Dictionary<string, CustomPatreonData>();

        private ReferenceHub hub;

        public PatreonRank Rank { get; set; }

        public CustomPatreonData Custom { get; set; }

        private int cycleIndex;

        private float time;

        private void Awake()
        {
            hub = GetComponent<ReferenceHub>();
            Custom = CustomData.TryGetValue(hub.characterClassManager.UserId, out var data) ? data : new CustomPatreonData();
        }

        private void OnDestroy()
        {
            CustomData[hub.characterClassManager.UserId] = Custom;
        }

        private void Update()
        {
            if (Custom.BadgeIndex != Badge.Cycle)
                return;
            float switchTime = PatreonPerksModule.Singleton.Config.AutoSwitchBadge;
            if (switchTime < 0)
                return;
            time += Time.deltaTime;
            if (time < switchTime)
                return;
            time = 0;
            if (++cycleIndex >= Rank.BadgeOptions.Count)
                cycleIndex = -1;
            string badge = cycleIndex < 0 && Custom.CustomBadge != null ? Custom.CustomBadge : Rank.BadgeOptions[cycleIndex].Content;
            string color = cycleIndex < 0 && Custom.CustomBadgeColor != null ? Custom.CustomBadgeColor : Rank.BadgeOptions[cycleIndex].Color;
            PatreonExtensions.SetRank(hub, badge, color);
        }

        public static PatreonData Get(ReferenceHub hub) => hub.TryGetComponent(out PatreonData data) ? data : hub.gameObject.AddComponent<PatreonData>();

        public void SetIndex(int result)
        {
            Custom.BadgeIndex = result + 1;
            cycleIndex = result;
            if (result == Badge.Custom)
            {
                PatreonExtensions.SetRank(hub, Custom.CustomBadge, Custom.CustomBadgeColor);
                return;
            }
            if (result < 0)
                return;
            PatreonExtensions.SetRank(hub, Rank.BadgeOptions[result].Content, Rank.BadgeOptions[result].Color);
        }
    }
}