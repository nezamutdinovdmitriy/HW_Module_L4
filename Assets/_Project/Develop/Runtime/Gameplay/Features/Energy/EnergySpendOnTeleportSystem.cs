using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

public class EnergySpendOnTeleportSystem : IInitializableSystem, IDisposableSystem
{
    private ReactiveEvent _teleporationEndEvent;

    private ReactiveVariable<float> _currentEnergy;
    private ReactiveVariable<float> _cost;

    private IDisposable _teleportationEndEventDisposable;

    public void OnInit(Entity entity)
    {
        _currentEnergy = entity.CurrentEnergy;

        _teleporationEndEvent = entity.TeleportationEndEvent;

        _teleportationEndEventDisposable = _teleporationEndEvent.Subscribe(OnTeleported);
    }

    private void OnTeleported() => _currentEnergy.Value -= _cost.Value;

    public void OnDispose() => _teleportationEndEventDisposable.Dispose();
}
