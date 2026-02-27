using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomTeleportationState : State, IUpdatableState
    {
        private readonly ReactiveEvent<Vector3> _teleportationStartRequest;
        private readonly float _teleportationRadius;

        public RandomTeleportationState(Entity entity, float radius)
        {
            _teleportationStartRequest = entity.TeleportationStartRequest;
            _teleportationRadius = radius;
        }

        public override void Enter()
        {
            base.Enter();

            _teleportationStartRequest?.Invoke(GetPosition());
        }

        public void Update(float deltaTime)
        {
        }

        private Vector3 GetPosition()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            Vector3 newPosition = new Vector3(randomDirection.x, 0, randomDirection.y) * _teleportationRadius;

            return newPosition;
        }
    }
}