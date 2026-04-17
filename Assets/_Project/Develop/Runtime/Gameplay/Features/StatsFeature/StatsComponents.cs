using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature
{
    public class BaseStats : IEntityComponent
    {
        public Dictionary<StatType, float> Value;
    }

    public class ModifiedStats : IEntityComponent
    {
        public Dictionary<StatType, float> Value;
    }

    public class StatsEffects : IEntityComponent
    {
        public StatsEffectsList Value;
    }
}