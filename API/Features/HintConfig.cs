using Exiled.API.Features;

namespace ScuutCore.API
{
    public class HintConfig
    {
        public HintConfig(string ms)
        {
            Message = ms;
        }

        public HintConfig()
        {
        }

        public string Message { get; set; }
        public int Time { get; set; } = 5;

        public void Show(Player ply = null)
        {
            if (ply != null)
                ply.ShowHint(Message, Time);
            else
                Map.ShowHint(Message, Time);
        }
    }
}