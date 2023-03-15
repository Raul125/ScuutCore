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

        public Badge(string content, string color)
        {
            Content = content;
            Color = color;
        }

    }
}