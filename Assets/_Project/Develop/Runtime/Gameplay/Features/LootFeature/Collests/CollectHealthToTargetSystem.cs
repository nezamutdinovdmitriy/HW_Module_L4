using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature.Collests
{
    public class CollectHealthToTargetSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<Entity> _target;
        private ReactiveVariable<float> _health;
        private ReactiveVariable<bool> _isCollected;

        private IDisposable _collectedChangeDisposable;

        public void OnInit(Entity entity)
        {
            _target = entity.CurrentTarget;
            _health = entity.CurrentHealth;
            _isCollected = entity.IsCollected;

            _collectedChangeDisposable = _isCollected.Subscribe(OnIsCollectedChanged);
        }

        public void OnDispose() => _collectedChangeDisposable?.Dispose();

        private void OnIsCollectedChanged(bool arg1, bool isCollected)
        {
            ReactiveVariable<float> currentHealth = _target.Value.CurrentHealth;
            ReactiveVariable<float> maxHealth = _target.Value.MaxHealth;

            if(currentHealth.Value + _health.Value > maxHealth.Value)
            {
                currentHealth.Value = maxHealth.Value;
                return;
            }

            currentHealth.Value += _health.Value;
        }
    }
}