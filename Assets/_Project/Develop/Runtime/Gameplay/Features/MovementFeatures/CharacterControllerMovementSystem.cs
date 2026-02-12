using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeatures
{
    public class CharacterControllerMovementSystem : IInitializableSystem, IUpdatableSystem
    {
        private CharacterController _characterController;

        private ReactiveVariable<Vector3> _direction;
        private ReactiveVariable<float> _moveSpeed;

        public void OnInit(Entity entity)
        {
            _characterController = entity.CharacterController;
            _direction = entity.RotationDirection;
            _moveSpeed = entity.MoveSpeed;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 direction = _direction.Value * _moveSpeed.Value;

            _characterController.Move(direction);
        }
    }
}