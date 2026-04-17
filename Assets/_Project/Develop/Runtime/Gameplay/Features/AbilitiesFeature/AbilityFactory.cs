using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature.Abilities;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature
{
    public class AbilityFactory
    {
        private DIContainer _container;

        public AbilityFactory(DIContainer container)
            => _container = container;

        public Ability CreateAbilityFor(Entity entity, AbilityConfig config)
        {
            switch (config)
            {
                case StatChangeAbilityConfig statChangeAbilityConfig:
                    return new StatChangeAbility(entity, statChangeAbilityConfig);

                default:
                    throw new ArgumentException();
            }
        }
    }
}