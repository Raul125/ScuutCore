using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.AdminTools
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}