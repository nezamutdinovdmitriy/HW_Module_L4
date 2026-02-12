using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

public class RigidbodyRotationSystem : IInitializableSystem, IUpdatableSystem
{
    private const float DeathZone = 0.05f;

    private ReactiveVariable<float> _rotateSpeed;
    private ReactiveVariable<Vector3> _direction;

    private Rigidbody _rigidbody;

    private ICompositeCondition _canRotate;

    public void OnInit(Entity entity)
    {
        _direction = entity.RotationDirection;
        _rotateSpeed = entity.RotationSpeed;
        _rigidbody = entity.Rigidbody;

        _canRotate = entity.CanRotate;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_canRotate.Evaluate() == false)
            return;

        if (_direction.Value.magnitude <= DeathZone)
            return;

        Quaternion lookRotaton = Quaternion.LookRotation(_direction.Value.normalized);

        float step = _rotateSpeed.Value * deltaTime;

        _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, lookRotaton, step));
    }
}
