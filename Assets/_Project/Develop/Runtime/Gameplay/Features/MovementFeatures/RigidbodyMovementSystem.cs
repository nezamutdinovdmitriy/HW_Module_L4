using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

public class RigidbodyMovementSystem : IInitializableSystem, IUpdatableSystem
{
    private ReactiveVariable<Vector3> _moveDirection;
    private ReactiveVariable<float> _moveSpeed;

    private Rigidbody _rigidbody;

    public void OnInit(Entity entity)
    {
        _moveDirection = entity.MoveDirection;
        _moveSpeed = entity.MoveSpeed;
        _rigidbody = entity.Rigidbody;
    }

    public void OnUpdate(float deltaTime)
    {
        Vector3 velocity = _moveDirection.Value.normalized * _moveSpeed.Value;

        _rigidbody.velocity = velocity;
    }
}
