namespace ScuutCore.API.Features
{
    using PluginAPI.Core;
    using PluginAPI.Core.Interfaces;

    public class ScuutPlayer : Player
    {
        public ScuutPlayer(IGameComponent component) : base(component)
        {
        }

        public Subclass SubClass { get; set; }

        public override void OnDestroy()
        {
        }
    }
}