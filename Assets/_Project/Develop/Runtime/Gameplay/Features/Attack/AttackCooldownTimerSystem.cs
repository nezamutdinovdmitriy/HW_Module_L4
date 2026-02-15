using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class AttackCooldownTimerSystem : IInitializableSystem, IDisposable, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<bool> _inAttackCooldown;

        private ReactiveEvent _endAttackEvent;

        private IDisposable _endAttackEventDisposable;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.AttackCooldownCurrentTime;
            _initialTime = entity.AttackCooldownInitialTime;
            _inAttackCooldown = entity.InAttackCooldown;

            _endAttackEvent = entity.EndAttackEvent;

            _endAttackEventDisposable = _endAttackEvent.Subscribe(OnEndAttack);
        }

        public void Dispose()
        {
            _endAttackEventDisposable.Dispose();
        }

        private void OnEndAttack()
        {
            Debug.Log("Attack Cooldown started!");

            _currentTime.Value = _initialTime.Value;
            _inAttackCooldown.Value = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inAttackCooldown.Value == false)
                return;

            _currentTime.Value -= deltaTime;

            if (CooldownIsOver())
            {
                _inAttackCooldown.Value = false;
                Debug.Log("Attack Cooldown ended!");
            }

        }

        private bool CooldownIsOver() => _currentTime.Value <= 0;
    }
}