using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature.SyncSystems
{
    public class AttackTimeByAttackSpeedStatSyncSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _attacksPerSecond;

        private ReactiveVariable<float> _attackProcessInitialTime;
        private ReactiveVariable<float> _attackProcessModifiedTime;
        private ReactiveVariable<float> _attackCooldownInitialTime;
        private ReactiveVariable<float> _attackCooldownModifiedTime;
        private ReactiveVariable<float> _attackDelayInitialTime;
        private ReactiveVariable<float> _attackDelayModifiedTime;

        private IDisposable _attacksPerSecondChangedDisposable;

        public void OnInit(Entity entity)
        {
            _attacksPerSecond = entity.AttacksPerSecond;

            _attackProcessInitialTime = entity.AttackProcessInitialTime;
            _attackProcessModifiedTime = entity.AttackProcessModifiedTime;
            _attackCooldownInitialTime = entity.AttackCooldownInitialTime;
            _attackCooldownModifiedTime = entity.AttackCooldownModifiedTime;
            _attackDelayInitialTime = entity.AttackDelayTime;
            _attackDelayModifiedTime = entity.AttackDelayModifiedTime;

            _attacksPerSecondChangedDisposable = _attacksPerSecond.Subscribe(OnAttacksPerSecondChanged);

            OnAttacksPerSecondChanged(0, _attacksPerSecond.Value);
        }

        private void OnAttacksPerSecondChanged(float arg1, float newAttacksPerSecond)
        {
            float totalBaseTime = _attackProcessInitialTime.Value + _attackCooldownInitialTime.Value;

            float targetTotalTime = 1f / newAttacksPerSecond;

            float totalTimeRation = targetTotalTime / totalBaseTime;

            _attackProcessModifiedTime.Value = _attackProcessInitialTime.Value * totalTimeRation;
            _attackCooldownModifiedTime.Value = _attackCooldownInitialTime.Value * totalTimeRation;
            _attackDelayModifiedTime.Value = _attackDelayInitialTime.Value * totalTimeRation;
        }

        public void OnDispose() => _attacksPerSecondChangedDisposable?.Dispose();
    }
}