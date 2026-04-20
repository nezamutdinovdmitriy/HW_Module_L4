using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature
{
    public class AbilityDroppingRulesService
    {
        public bool IsAvailable(AbilityConfig config, Entity entity, int abilityLevel)
        {
            if (config.IsUpgradable())
            {
                if(entity.Abilities.Elements.Any(ability =>
                ability.ID == config.ID
                && ability.CurrentLevel.Value + abilityLevel > ability.MaxLevel))
                {
                    return false;
                }
            }

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