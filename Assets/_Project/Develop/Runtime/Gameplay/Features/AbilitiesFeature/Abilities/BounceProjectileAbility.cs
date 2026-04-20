using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Gameplay.Features.BounceFeature;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature.Abilities
{
    public class BounceProjectileAbility : Ability, IDisposable
    {
        private BounceProjectileAbilityConfig _config;
        private Entity _owner;
        private EntitiesLifeContext _lifeContext;

        public BounceProjectileAbility(
            BounceProjectileAbilityConfig config,
            Entity owner,
            EntitiesLifeContext lifeContext,
            int currentLevel)
            : base(config.ID, currentLevel, config.MaxLevel)
        {
            _config = config;
            _owner = owner;
            _lifeContext = lifeContext;
        }

        public override void Activate() => _lifeContext.Added += OnCreaturesAdded;

        public void Dispose() => _lifeContext.Added -= OnCreaturesAdded;

        private void OnCreaturesAdded(Entity entity)
        {
            if(entity.HasComponent<IsProjectile>()
                && entity.TryGetOwner(out ReactiveVariable<Entity> owner)
                && owner.Value == _owner)
            {
                entity
                    .AddBounceCount(new ReactiveVariable<int>(_config.GetBounceCountBy(CurrentLevel.Value)))
                    .AddBounceEvent()
                    .AddLayerToBounceReaction(_config.LayerBounceRection);

                entity.MustDie.Add(new FuncCondition(() => entity.BounceCount.Value + 1 == 0), 5);

                entity
                    .AddSystem(new BounceDetectorSystem())
                    .AddSystem(new ReflectRotationDirectionOnBounceSystem())
                    .AddSystem(new ReflectMovementDirectionOnBounceSystem())
                    .AddSystem(new BounceCountDecreaseSystem());
            }
        }
    }
}