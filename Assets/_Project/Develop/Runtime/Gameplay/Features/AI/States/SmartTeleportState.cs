using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

public class SmartTeleportState : State, IUpdatableState
{
    private readonly ReactiveEvent<Vector3> _teleportaionStartRequest;
    private readonly ReactiveVariable<Entity> _currentTarget;

    public SmartTeleportState(Entity entity)
    {
        _teleportaionStartRequest = entity.TeleportationStartRequest;
        _currentTarget = entity.CurrentTarget;
    }

    public override void Enter()
    {
        base.Enter();

        _teleportaionStartRequest?.Invoke(_currentTarget.Value.Transform.position);

        Debug.Log(_currentTarget.Value.Transform.position);
    }

    public void Update(float deltaTime)
    {
    }
}
