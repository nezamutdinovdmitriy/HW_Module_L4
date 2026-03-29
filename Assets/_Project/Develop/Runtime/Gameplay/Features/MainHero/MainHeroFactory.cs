using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MainHero
{
    public class MainHeroFactory
    {
        private readonly DIContainer _container;

        private readonly EntitiesFactory _entitiesFactory;
        private readonly BrainsFactory _brainsFactory;

        private readonly EntitiesLifeContext _entitiesLifeContext;

        private readonly ConfigsProviderService _configProviderService;

        public MainHeroFactory(DIContainer container)
        {
            _container = container;

            _entitiesFactory = container.Resolve<EntitiesFactory>();
            _brainsFactory = container.Resolve<BrainsFactory>();
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
            _configProviderService = container.Resolve<ConfigsProviderService>();
        }

        public Entity Create(Vector3 position)
        {
            HeroConfig config = _configProviderService.GetConfig<HeroConfig>();
            
            Entity entity = _entitiesFactory.CreateHeroAlternative2(position, config);

            entity
                .AddIsMainHero()
                .AddCurrentTarget()
                .AddTeam(new ReactiveVariable<TeamType>(TeamType.MainHero));

            _brainsFactory.CreateMainHeroBrain(entity, new NearestDamageableTargetSelector(entity));

            _entitiesLifeContext.Add(entity);

            return entity; 
        }
    }
}