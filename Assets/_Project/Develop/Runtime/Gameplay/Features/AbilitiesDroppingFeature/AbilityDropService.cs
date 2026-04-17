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

        public List<AbilityConfig> Drop(int count, Entity entity)
        {
            List<AbilityConfig> availablesAbilities = new(
                _container
                    .AbilityConfigs
                    .Where(option => _dropRules.IsAvailable(option, entity)));

            List<AbilityConfig> selectedAbilities = new();

            for (int i = 0; i < count; i++)
            {
                AbilityConfig selectedAbility = availablesAbilities[Random.Range(0, availablesAbilities.Count)];
                selectedAbilities.Add(selectedAbility);
                availablesAbilities.Remove(selectedAbility);
            }

            return selectedAbilities;
        }
    }
}