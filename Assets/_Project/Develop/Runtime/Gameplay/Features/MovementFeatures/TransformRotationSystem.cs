using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

public class TransformRotationSystem : IInitializableSystem, IUpdatableSystem
{
    private const float DeathZone = 0.05f;

    private Transform _transform;

    private ReactiveVariable<float> _rotationSpeed;
    private ReactiveVariable<Vector3> _currentDirection;

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _rotationSpeed = entity.RotationSpeed;
        _currentDirection = entity.MoveDirection;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_currentDirection.Value.magnitude <= DeathZone)
            return;

        Quaternion lookRotaton = Quaternion.LookRotation(_currentDirection.Value.normalized);

        float step = _rotationSpeed.Value * deltaTime;

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, lookRotaton, step);
    }
}
