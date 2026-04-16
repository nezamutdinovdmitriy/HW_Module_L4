using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature
{
    public interface IStatsEffect
    {
        public void ApplyTo(Dictionary<StatsType, float> stats);
    }
}