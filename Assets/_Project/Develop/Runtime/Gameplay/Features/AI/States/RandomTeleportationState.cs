using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomTeleportationState : State, IUpdatableState
    {
        private readonly ReactiveEvent _teleportationStartRequest;

        public RandomTeleportationState(Entity entity)
        {
            _teleportationStartRequest = entity.TeleportationStartRequest;
        }

        public override void Enter()
        {
            base.Enter();

            _teleportationStartRequest?.Invoke();
        }

        public void Update(float deltaTime)
        {
        }
    }
}