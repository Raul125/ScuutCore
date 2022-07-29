using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.Subclasses
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;


    }
}