using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature.SyncSystems
{
    public class MoveSpeedStatSyncSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _speed;
        private Dictionary<StatsType, float> _modifiedStats;

        public void OnInit(Entity entity)
        {
            _speed = entity.MoveSpeed;
            _modifiedStats = entity.ModifiedStats;
        }

        public void OnUpdate(float deltaTime)
        {
            float tempValue = _modifiedStats[StatsType.MoveSpeed];

            if(tempValue < 0)
                tempValue = 0;

            _speed.Value = tempValue;
        }
    }
}