using PluginAPI.Core;
using ScuutCore.API;
using System.ComponentModel;
using System.IO;

namespace ScuutCore.Modules.KillMessages
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public string Message { get; set; } = "Killed <color=red>{name}</color>";
    }
}