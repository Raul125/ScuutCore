namespace ScuutCore.Modules.Patreon.Types
{
    using System;
    [Serializable]
    public struct Badge
    {

        public const int Cycle = 0;

        public const int Custom = -1;

        public string Content { get; set; }

        public string Color { get; set; }

        public string[] RainbowColors { get; set; }

        public Badge(string content, string color, string[] rainbowColors = null)
        {
            Content = content;
            Color = color;
            RainbowColors = rainbowColors;
        }

    }
}