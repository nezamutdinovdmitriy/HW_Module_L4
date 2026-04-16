using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature
{
    public class StatsEffect : IStatsEffect
    {
        private StatsType _statType;
        private Func<float, float> _applyEffect;

        public StatsEffect( 
            StatsType statType, 
            Func<float, float> applyEffect)
        {
            _statType = statType;
            _applyEffect = applyEffect;
        }

        public void ApplyTo(Dictionary<StatsType, float> stats)
        {
            stats[_statType] = _applyEffect(stats[_statType]);
        }
    }
}