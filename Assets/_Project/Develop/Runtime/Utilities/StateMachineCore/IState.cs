using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Utilities.StateMachineCore
{
    public interface IState
    {
        public IReadOnlyEvent Entered { get; }
        public IReadOnlyEvent Exited { get; }

        public void Enter();
        public void Exit();
    }
}