namespace ScuutCore.Modules.BetterCandy
{
    using System.ComponentModel;
    using API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Number that the plugin will randomize between, like if the number is 5 it will be: 1-5")]
        public int MaxRandomizer { get; set; } = 10;

        [Description("The number that has to be choosen when the plugin randomizes for the player to receive the pink candy")]
        public int ChoosenNumber { get; set; } = 5;
    }
}