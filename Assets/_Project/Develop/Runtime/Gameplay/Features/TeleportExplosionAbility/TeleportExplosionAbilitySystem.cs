using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

public class TeleportExplosionAbilitySystem : IInitializableSystem, IDisposableSystem
{
    private ReactiveEvent _teleportationEndEvent;
    private ReactiveEvent _explosionStartRequest;

    private IDisposable _teleportationEndEventDisposable;

    public void OnInit(Entity entity)
    {
        _teleportationEndEvent = entity.TeleportationEndEvent;
        _explosionStartRequest = entity.ExplosionStartRequest;

        _teleportationEndEventDisposable = _teleportationEndEvent.Subscribe(OnTeleportationEnded);
    }

    public void OnDispose()
    {
        _teleportationEndEventDisposable.Dispose();
    }

    private void OnTeleportationEnded()
    {
        _explosionStartRequest?.Invoke();
    }

}
