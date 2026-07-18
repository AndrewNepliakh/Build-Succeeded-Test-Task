namespace Services.StateMachines
{
    public interface IUpdatableState
    {
        void Update(float deltaTime);
    }
}