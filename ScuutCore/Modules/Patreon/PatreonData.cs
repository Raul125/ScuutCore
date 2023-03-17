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
            GetBadge(out string badge, out string color);
            PatreonExtensions.SetRank(hub, badge, color);
        }

        public static PatreonData Get(ReferenceHub hub) => hub.TryGetComponent(out PatreonData data) ? data : hub.gameObject.AddComponent<PatreonData>();

        private void GetBadge(out string text, out string color)
        {
            if (Custom.BadgeIndex < 0)
            {
                if (Custom.CustomBadge == null)
                    Custom.BadgeIndex = Badge.Cycle;
                else
                {
                    text = Custom.CustomBadge;
                    color = Custom.CustomBadgeColor ?? "white";
                    return;
                }
            }

            if (Custom.BadgeIndex == Badge.Cycle)
            {
                if (++cycleIndex >= Rank.BadgeOptions.Count)
                    cycleIndex = 0;
                text = Rank.BadgeOptions[cycleIndex].Content;
                color = Rank.BadgeOptions[cycleIndex].Color;
                return;
            }

            if (Custom.BadgeIndex > Rank.BadgeOptions.Count)
                Custom.BadgeIndex = 1;
            text = Rank.BadgeOptions[Custom.BadgeIndex - 1].Content;
            color = Rank.BadgeOptions[Custom.BadgeIndex - 1].Color;
        }

        public void SetIndex(int result)
        {
            Custom.BadgeIndex = result + 1;
            cycleIndex = result;
            GetBadge(out string badge, out string color);
            PatreonExtensions.SetRank(hub, badge, color);
        }
    }
}