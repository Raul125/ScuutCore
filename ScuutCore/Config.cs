using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.IO;

namespace ScuutCore.Config
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string ConfigsFolder = Path.Combine(Paths.Configs, "ScuutCore");
        public bool Debug { get; set; } = false;
    }
}
