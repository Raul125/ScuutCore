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

        private static readonly Dictionary<string, PatreonPreferences> Preferences = new Dictionary<string, PatreonPreferences>();

        public ReferenceHub Hub { get; private set; }

        private PatreonRank rank;

        public PatreonRank Rank
        {
            get => rank;
            set
            {
                if (!value.IsValid)
                    return;
                rank = value;
                UpdateBadge();
            }
        }

        public PatreonPreferences Prefs { get; set; } = new PatreonPreferences();

        private int cycleIndex;

        private float time;

        private void Awake()
        {
            Hub = GetComponent<ReferenceHub>();
            Prefs = Preferences.TryGetValue(Hub.characterClassManager.UserId, out var data) ? data : new PatreonPreferences();
        }

        private void OnDestroy() => Preferences[Hub.characterClassManager.UserId] = Prefs;

        private void Update()
        {
            if (Prefs.BadgeIndex != Badge.Cycle)
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
            if (Prefs.BadgeIndex < 0)
            {
                if (Prefs.CustomBadge == null)
                    Prefs.BadgeIndex = Badge.Cycle;
                else
                {
                    text = Prefs.CustomBadge;
                    color = Prefs.CustomBadgeColor ?? "white";
                    return;
                }
            }

            if (Prefs.BadgeIndex == Badge.Cycle)
            {
                if (++cycleIndex >= Rank.BadgeOptions.Count)
                {
                    cycleIndex = -1;
                    if (Prefs.CustomBadge != null)
                    {
                        text = Prefs.CustomBadge;
                        color = Prefs.CustomBadgeColor ?? "white";
                        return;
                    }
                    cycleIndex = 0;
                }

                text = Rank.BadgeOptions[cycleIndex].Content;
                color = Rank.BadgeOptions[cycleIndex].Color;
                return;
            }

            if (Prefs.BadgeIndex > Rank.BadgeOptions.Count)
                Prefs.BadgeIndex = 1;
            text = Rank.BadgeOptions[Prefs.BadgeIndex - 1].Content;
            color = Rank.BadgeOptions[Prefs.BadgeIndex - 1].Color;
        }

        public void SetIndex(int result)
        {
            cycleIndex = Prefs.BadgeIndex = result;
            UpdateBadge();
        }
        public static void WriteAll()
        {
            try
            {
                foreach (var component in FindObjectsOfType<PatreonData>())
                    component.OnDestroy();
                string data = Loader.Serializer.Serialize(Preferences);
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
                var dictionary = Loader.Deserializer.Deserialize<Dictionary<string, PatreonPreferences>>(text);
                Preferences.Clear();
                foreach (var pair in dictionary)
                    Preferences.Add(pair.Key, pair.Value);
            }
            catch (Exception e)
            {
                Log.Error("Failed reading Patreon data preferences: " + e);
            }
        }

        private static string DataPath => Path.Combine(Plugin.Singleton.Config.ConfigsFolder, PatreonPerksModule.Singleton.Name, "preferences.yml");

        public void UpdateBadge()
        {
            GetBadge(out string badge, out string color);
            PatreonExtensions.SetRank(Hub, badge, color);
        }
    }
}