using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Features;

namespace ScuutCore.Modules.Subclasses
{
    public class Subclass
    {
        public string Name { get; set; }
        public Exiled.API.Features.Broadcast Broadcast { get; set; }
        public List<string> Inventory { get; set; }
        public float Hp { get; set; }

    }
}