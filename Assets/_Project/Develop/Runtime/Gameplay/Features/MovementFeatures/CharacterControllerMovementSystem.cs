using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeatures
{
    public class CharacterControllerMovementSystem : IInitializableSystem, IUpdatableSystem
    {
        private CharacterController _characterController;

        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _moveSpeed;

        public void OnInit(Entity entity)
        {
            _characterController = entity.CharacterController;
            _moveDirection = entity.MoveDirection;
            _moveSpeed = entity.MoveSpeed;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 direction = _moveDirection.Value * _moveSpeed.Value;

            _characterController.Move(direction);
        }
    }
}