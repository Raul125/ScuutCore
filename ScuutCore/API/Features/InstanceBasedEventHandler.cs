namespace ScuutCore.API.Features
{
    public abstract class InstanceBasedEventHandler<TModule> : IEventHandler
    {
        public TModule Module { get; set; }

    }
}