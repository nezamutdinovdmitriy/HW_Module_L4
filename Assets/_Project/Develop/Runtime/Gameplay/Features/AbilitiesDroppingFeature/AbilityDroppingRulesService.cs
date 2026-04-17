using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature
{
    public class AbilityDroppingRulesService
    {
        public bool IsAvailable(AbilityConfig config, Entity entity)
        {
            switch (config)
            {
                case StatChangeAbilityConfig statChangeAbilityConfig:
                    return entity.TryGetModifiedStats(out Dictionary<StatType, float> modifiedStats)
                        && modifiedStats.ContainsKey(statChangeAbilityConfig.StatType);
            }

            return true;
        }
    }
}