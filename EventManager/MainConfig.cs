using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace EventManager
{
    public class MainConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public int MinPlayers { get; set; } = 3;
        
        [Description("The donators RA groups and their cooldown (in seconds).")]
        public Dictionary<string, long> DonationGroups { get; set; } = new Dictionary<string, long> {["owner"] = -1, ["patron1"] = 3600, ["patron2"] = 18000};

        [Description("The possible event file names and the minimum players")]
        public Dictionary<string, int> Events { get; set; } = new Dictionary<string, int> {["murder"] = 0};
    }
}