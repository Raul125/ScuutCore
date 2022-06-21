using LiteDB;

namespace ScuutCore.Modules.KillMessages
{
    public class MessageData
    {
        [BsonId]
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool Disabled { get; set; }
        public string Color { get; set; }
    }
}