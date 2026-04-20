using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature.SyncSystems
{
    public class AttacksPerSecondStatSyncSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _attaksPerSecond;
        private Dictionary<StatType, float> _modifiedStats;

        public void OnInit(Entity entity)
        {
            _attaksPerSecond = entity.AttacksPerSecond;
            _modifiedStats = entity.ModifiedStats;
        }

        public void OnUpdate(float deltaTime)
        {
            float tempValue = _modifiedStats[StatType.AttackPerSecond];

            if(tempValue < 0)
                tempValue = 0;

            _attaksPerSecond.Value = tempValue;
        }
    }
}