namespace Assets._Project.Develop.Runtime.Utilities.StateMachineCore
{
    public interface IUpdatableState : IState
    {
        public void Update(float deltaTime);
    }
}