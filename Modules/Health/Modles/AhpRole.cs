namespace ScuutCore.Modules.Health
{
    public class AhpRole
    {
        public float Amount { get; set; } = 10;
        public float Limit { get; set; } = 10;
        public float Decay { get; set; } = 10;
        public float Efficacy { get; set; } = 10;
        public float Sustain { get; set; } = 10;
        public bool Persistent { get; set; } = true;
    }
}