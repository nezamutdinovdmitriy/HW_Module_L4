using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class EnergySystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _currentEnergy;

        private ReactiveVariable<float> _energyRegenTickInterval;
        private ReactiveVariable<float> _energyRegenCurrentTime;

        private ReactiveVariable<float> _energyRegenPerTick;

        public void OnInit(Entity entity)
        {
            _maxEnergy = entity.MaxEnergy;
            _currentEnergy = entity.CurrentEnergy;
            _energyRegenTickInterval = entity.EnergyRegenTickInterval;
            _energyRegenCurrentTime = entity.EnergyRegenCurrentTime;
            _energyRegenPerTick = entity.EnergyRegenValuePerTick;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_currentEnergy.Value == _maxEnergy.Value)
                return;

            _energyRegenCurrentTime.Value -= deltaTime;

            if(_energyRegenCurrentTime.Value <= 0)
            {
                float regenAmount = _maxEnergy.Value * (_energyRegenPerTick.Value / 100f);

                _currentEnergy.Value = Mathf.Min(_currentEnergy.Value + regenAmount, _maxEnergy.Value);

                _energyRegenCurrentTime.Value = _energyRegenTickInterval.Value;
            }
        }
    }
}