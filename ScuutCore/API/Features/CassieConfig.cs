namespace ScuutCore.API
{
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
        public string customSubtitle { get; set; } = "";

        public void Play()
        {
            if (customSubtitle != "")
                Exiled.API.Features.Cassie.MessageTranslated(Text, customSubtitle, isHeld, isNoisy, isSubtitles);
            else
                Exiled.API.Features.Cassie.Message(Text, isHeld, isNoisy, isSubtitles);
        }
    }
}