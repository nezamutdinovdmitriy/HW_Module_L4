using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RotateToTargetState : State, IUpdatableState
    {
        private readonly ReactiveVariable<Vector3> _rotationDirection;
        private readonly ReactiveVariable<Entity> _currentTarget;
        private readonly Transform _transform;

        public RotateToTargetState(Entity entity)
        {
            _rotationDirection = entity.RotationDirection;
            _currentTarget = entity.CurrentTarget;
            _transform = entity.Transform;
        }

        public void Update(float deltaTime)
        {
            if (_currentTarget != null)
                _rotationDirection.Value = (_currentTarget.Value.Transform.position - _transform.position).normalized;
        }
    }
}