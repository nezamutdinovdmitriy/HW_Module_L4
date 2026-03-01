using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

public class AimingState : State, IUpdatableState
{
    private readonly ScreenToWorldPositionConverter _screenToWorldPositionConverter;
    private readonly Transform _entityTransform;
    private readonly ReactiveVariable<Vector3> _rotationDirection;
    private readonly IInputService _input;

    public AimingState(Entity entity, ScreenToWorldPositionConverter screenToWorldPositionConverter, IInputService inputService)
    {
        _entityTransform = entity.Transform;
        _rotationDirection = entity.RotationDirection;

        _screenToWorldPositionConverter = screenToWorldPositionConverter;
        
        _input = inputService;
    }

    public void Update(float deltaTime)
    {
        Vector3 direction = _screenToWorldPositionConverter.GetPosition(_input.Aim.Value, _entityTransform.transform.position.y);

        _rotationDirection.Value = direction - _entityTransform.transform.position;
    }
}
