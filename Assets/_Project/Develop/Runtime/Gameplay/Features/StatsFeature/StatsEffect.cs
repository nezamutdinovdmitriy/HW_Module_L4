using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature
{
    public class StatsEffect : IStatsEffect
    {
        private StatType _statType;
        private Func<float, float> _applyEffect;

        public StatsEffect( 
            StatType statType, 
            Func<float, float> applyEffect)
        {
            _statType = statType;
            _applyEffect = applyEffect;
        }

        public void ApplyTo(Dictionary<StatType, float> stats)
        {
            stats[_statType] = _applyEffect(stats[_statType]);
        }
    }
}