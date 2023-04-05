namespace ScuutCore.Modules.Pets
{
    using API.Features;

    public sealed class Pets : EventControllerModule<Pets, Config, EventHandlers>
    {
        public override void OnEnabled()
        {
            CharacterClassManager.OnInstanceModeChanged += OnInstanceModeChanged;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CharacterClassManager.OnInstanceModeChanged -= OnInstanceModeChanged;
            base.OnDisabled();
        }

        public void OnInstanceModeChanged(ReferenceHub referenceHub, ClientInstanceMode instanceMode)
        {
            foreach (Pet pet in Pet.List)
            {
                if (instanceMode is not ClientInstanceMode.Unverified or ClientInstanceMode.Host && pet.ReferenceHub == referenceHub)
                    referenceHub.characterClassManager.InstanceMode = ClientInstanceMode.Host;
            }
        }
    }
}