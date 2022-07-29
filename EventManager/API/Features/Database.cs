using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Exiled.API.Features;

namespace EventManager.Api
{
    public static class Database
    {
        private static readonly string FolderPath = Path.Combine(Paths.Configs, "EventManager");
        private static readonly string DbPath = Path.Combine(FolderPath, "User.db");

        public static Dictionary<string, long> Cache = new Dictionary<string, long>();

        public static void Load()
        {
            if (!File.Exists(DbPath))
            {
                Directory.CreateDirectory(FolderPath);
                Save(new Dictionary<string, long>());
            }

            Cache = Utf8Json.JsonSerializer.Deserialize<Dictionary<string, long>>(File.ReadAllText(DbPath));
        }

        public static void Save(Dictionary<string, long> users)
        {
            Cache = users;
            Save();
        }

        public static void Save()
        {
            File.WriteAllText(DbPath, Encoding.UTF8.GetString(Utf8Json.JsonSerializer.Serialize(Cache)));
        }

        public static bool IsInCooldown(Player player) => IsInCooldown(player.UserId, player.GroupName);

        public static bool IsInCooldown(string userId, string userGroup)
        {
            if (!Cache.ContainsKey(userId))
                return false;

            return Cache[userId] > DateTime.UtcNow.Ticks - MainClass.Instance.Config.DonationGroups[userGroup];
        }
        
        public class User
        {
            public string UserId { get; set; }
            public long LastUsed { get; set; }
        }
    }
}