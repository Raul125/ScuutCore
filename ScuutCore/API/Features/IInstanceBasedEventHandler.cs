namespace ScuutCore.API.Features
{
    public interface IInstanceBasedEventHandler<in TModule> : IEventHandler
    {
        void AssignModule(TModule module);
    }
}