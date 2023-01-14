namespace ScuutCore.API.Features
{
    using PluginAPI.Core;

    public class CassieConfig
    {
        public CassieConfig(string ms)
        {
            Text = ms;
        }

        public CassieConfig()
        {
        }

        public string Text { get; set; } = "";
        public bool isSubtitles { get; set; } = false;
        public bool isHeld { get; set; } = false;
        public bool isNoisy { get; set; } = false;

        public void Play()
        {
            Cassie.Message(Text, isHeld, isNoisy, isSubtitles);
        }
    }
}