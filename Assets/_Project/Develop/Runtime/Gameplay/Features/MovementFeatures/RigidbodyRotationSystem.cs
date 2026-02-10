using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

public class RigidbodyRotationSystem : IInitializableSystem, IUpdatableSystem
{
    private const float DeathZone = 0.05f;

    private ReactiveVariable<float> _rotateSpeed;
    private ReactiveVariable<Vector3> _currentDirection;

    private Rigidbody _rigidbody;

    public void OnInit(Entity entity)
    {
        _currentDirection = entity.MoveDirection;
        _rotateSpeed = entity.RotationSpeed;
        _rigidbody = entity.Rigidbody;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_currentDirection.Value.magnitude <= DeathZone)
            return;

        Quaternion lookRotaton = Quaternion.LookRotation(_currentDirection.Value.normalized);

        float step = _rotateSpeed.Value * deltaTime;

        _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, lookRotaton, step));
    }
}
