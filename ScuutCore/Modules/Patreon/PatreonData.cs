namespace ScuutCore.Modules.Patreon
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using API.Loader;
    using PluginAPI.Core;
    using Types;
    using UnityEngine;

    public sealed class PatreonData : MonoBehaviour
    {

        private static readonly Dictionary<string, CustomPatreonData> CustomData = new Dictionary<string, CustomPatreonData>();

        public ReferenceHub Hub { get; private set; }

        private PatreonRank rank;

        public PatreonRank Rank
        {
            get => rank;
            set
            {
                rank = value;
                UpdateBadge();
            }
        }

        public CustomPatreonData Custom { get; set; }

        private int cycleIndex;

        private float time;

        private void Awake()
        {
            Hub = GetComponent<ReferenceHub>();
            Custom = CustomData.TryGetValue(Hub.characterClassManager.UserId, out var data) ? data : new CustomPatreonData();
        }

        private void OnDestroy() => CustomData[Hub.characterClassManager.UserId] = Custom;

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
            UpdateBadge();
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
            cycleIndex = Custom.BadgeIndex = result;
            UpdateBadge();
        }
        public static void WriteAll()
        {
            try
            {
                foreach (var component in FindObjectsOfType<PatreonData>())
                    component.OnDestroy();
                string data = Loader.Serializer.Serialize(CustomData);
                File.WriteAllText(DataPath, data);
            }
            catch (Exception e)
            {
                Log.Error("Failed writing data preferences: " + e);
            }
        }

        public static void ReadAll()
        {
            try
            {
                if (!File.Exists(DataPath))
                    return;
                string text = File.ReadAllText(DataPath);
                var dictionary = Loader.Deserializer.Deserialize<Dictionary<string, CustomPatreonData>>(text);
                CustomData.Clear();
                foreach (var pair in dictionary)
                    CustomData.Add(pair.Key, pair.Value);
            }
            catch (Exception e)
            {
                Log.Error("Failed reading data preferences: " + e);
            }
        }

        private static string DataPath => Path.Combine(Plugin.Singleton.Config.ConfigsFolder, PatreonPerksModule.Singleton.Name, "data.yml");
        public void UpdateBadge()
        {
            GetBadge(out string badge, out string color);
            PatreonExtensions.SetRank(Hub, badge, color);
        }
    }
}