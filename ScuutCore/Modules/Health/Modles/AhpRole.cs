namespace ScuutCore.Modules.Health
{
    public class AhpRole
    {
        public float Amount { get; set; } = 10;
        public float Limit { get; set; } = 10;
        public float Decay { get; set; } = 0;
        public float Efficacy { get; set; } = 10;
        public bool Persistent { get; set; } = true;
        public float SustainTime { get; set; } = -10;
    }
}