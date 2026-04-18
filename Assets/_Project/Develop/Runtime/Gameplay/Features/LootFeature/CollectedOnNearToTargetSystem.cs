using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature
{
    public class CollectedOnNearToTargetSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly float _collectDistance;

        private ReactiveVariable<Entity> _target;
        private Transform _transform;
        private ReactiveVariable<bool> _isCollected;

        public CollectedOnNearToTargetSystem(float collectDistance) => _collectDistance = collectDistance;

        public void OnInit(Entity entity)
        {
            _target = entity.CurrentTarget;
            _transform = entity.Transform;
            _isCollected = entity.IsCollected;
        }

        public void OnUpdate(float deltaTime)
        {
            if(_isCollected.Value == false && _target.Value != null)
                if((_target.Value.Transform.position - _transform.position).magnitude < _collectDistance)
                {
                    _isCollected.Value = true;
                    Debug.Log("COLLECTED ++");
                }
        }
    }
}