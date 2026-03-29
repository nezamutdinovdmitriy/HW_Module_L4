using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpawnFeature
{
    public class SpawnProcessTimerSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<float> _currentTime;

        private ReactiveVariable<bool> _spawnInProcess;

        public void OnInit(Entity entity)
        {
            _initialTime = entity.SpawnInitialTime;
            _currentTime = entity.SpawnCurrentTime;
            _spawnInProcess = entity.SpawnInProcess;

            _currentTime.Value = 0;
            _spawnInProcess.Value = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_spawnInProcess.Value == false)
                return;

            _currentTime.Value += deltaTime;

            if(_currentTime.Value >= _initialTime.Value)
                _spawnInProcess.Value = false;
        }
    }
}