namespace ScuutCore.Modules.Patreon.Types
{
    using System;
    [Serializable]
    public struct Badge
    {
        public string Content { get; set; }

        public string Color { get; set; }

        public Badge(string content, string color)
        {
            Content = content;
            Color = color;
        }
    }
}