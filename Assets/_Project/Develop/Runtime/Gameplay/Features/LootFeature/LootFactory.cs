using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature.Collests;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature
{
    public class LootFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly MonoEntitiesFactory _monoEntitiesFactory;

        public LootFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
            _monoEntitiesFactory = container.Resolve<MonoEntitiesFactory>();
        }

        public Entity CreateExperienceLoot(string prefabPath, Vector3 position, float experience)
        {
            Entity loot = CreatePullable(prefabPath, position);

            loot
                .AddExperience(new(experience))
                .AddSystem(new CollectExperienceToTargetSystem());

            _entitiesLifeContext.Add(loot);

            return loot;
        }

        public Entity CreateCoinsLoot(string prefabPath, Vector3 position, int coins)
        {
            Entity loot = CreatePullable(prefabPath, position);

            loot
                .AddCoins(new(coins))
                .AddSystem(new CollectCoinsToTargetSystem());

            _entitiesLifeContext.Add(loot);

            return loot;
        }

        public Entity CreateHealthLoot(string prefabPath, Vector3 position, float health)
        {
            Entity loot = CreatePullable(prefabPath, position);

            loot
                .AddCurrentHealth(new(health))
                .AddSystem(new CollectHealthToTargetSystem());

            _entitiesLifeContext.Add(loot);

            return loot;
        }

        private Entity CreatePullable(string prefabPath, Vector3 position)
        {
            Entity entity = new();

            float collectDistance = 0.3f;

            _monoEntitiesFactory.Create(entity, position, prefabPath);

            ICompositeCondition moveCondition = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsPullingProcess.Value))
                .Add(new FuncCondition(() => entity.SpawnInProcess.Value == false));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsCollected.Value));

            entity
                .AddIsPullable()
                .AddIsPullingProcess()
                .AddSpawnInProcess(new(true))
                .AddCurrentTarget(new(null))
                .AddMoveDirection()
                .AddMoveSpeed(new(12))
                .AddIsMoving()
                .AddIsCollected()
                .AddCanMove(moveCondition)
                .AddMustSelfRelease(mustSelfRelease)
                .AddSystem(new GenerateMoveDirectionToTargetSystem())
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new CollectedOnNearToTargetSystem(collectDistance))
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }
    }
}