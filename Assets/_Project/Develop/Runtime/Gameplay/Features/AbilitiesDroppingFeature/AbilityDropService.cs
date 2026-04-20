using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature
{
    public class AbilityDropService
    {
        private readonly AbilityDroppingRulesService _dropRules;
        private readonly AbilitiesConfigsContainer _container;

        public AbilityDropService(
            AbilityDroppingRulesService dropRules,
            AbilitiesConfigsContainer container)
        {
            _dropRules = dropRules;
            _container = container;
        }

        public List<AbilityDropOption> Drop(int count, Entity entity)
        {
            List<AbilityDropOption> availablesAbilities = new();

            foreach (AbilityConfig abilityConfig in _container.AbilityConfigs)
            {
                for (int level = 1; level < abilityConfig.MaxLevel + 1; level++)
                {
                    if(_dropRules.IsAvailable(abilityConfig, entity, level))
                        availablesAbilities.Add(new AbilityDropOption(abilityConfig, level));
                }
            }

            List<AbilityDropOption> selectedAbilities = new();

            for (int i = 0; i < count; i++)
            {
                AbilityDropOption selectedAbility = availablesAbilities[Random.Range(0, availablesAbilities.Count)];
                selectedAbilities.Add(selectedAbility);
                availablesAbilities.Remove(selectedAbility);
            }

            return selectedAbilities;
        }
    }
}