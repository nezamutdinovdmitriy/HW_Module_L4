using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot;
using Assets._Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.Energy;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.Sensors;
using Assets._Project.Develop.Runtime.Gameplay.Features.Teleportation;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Optimization;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;

        private readonly EntitiesLifeContext _entitiesLifeContext;

        private readonly MonoEntitiesFactory _monoEntitiesFactory;

        private readonly CollidersRegistryService _collidersRegistryService;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;

            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();

            _monoEntitiesFactory = _container.Resolve<MonoEntitiesFactory>();

            _collidersRegistryService = _container.Resolve<CollidersRegistryService>();
        }

        public Entity CreateGhost(Vector3 position)
        {
            Entity entity = CreateEmpty();
            MonoEntity monoEntity = _monoEntitiesFactory.Create(entity, position, "Entities/Characters/Ghost");

            entity
                .AddMoveDirection()
                .AddRotationDirection()
                .AddMoveSpeed(new ReactiveVariable<float>(7))
                .AddIsMoving()
                .AddRotationSpeed(new ReactiveVariable<float>(850))
                .AddMaxHealth(new ReactiveVariable<float>(100))
                .AddCurrentHealth(new ReactiveVariable<float>(100))
                .AddIsDead()
                .AddInDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                .AddDeathProcessCurrentTime()
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddContactsDetectingMask(UnityLayersAPI.LayerMaskCharacters)
                .AddContactsCollidersBuffer(new Buffer<Collider>(64))
                .AddContactsEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(50));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage);

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem())
                .AddSystem(new BodyContactsDetectingSystem())
                .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                .AddSystem(new DealDamageOnContactSystem())
                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateHero(Vector3 position)
        {
            Entity entity = CreateEmpty();
            MonoEntity monoEntity = _monoEntitiesFactory.Create(entity, position, "Entities/Characters/Hero");

            entity
                .AddMoveDirection()
                .AddRotationDirection()
                .AddMoveSpeed(new ReactiveVariable<float>(7))
                .AddIsMoving()
                .AddRotationSpeed(new ReactiveVariable<float>(850))
                .AddMaxHealth(new ReactiveVariable<float>(100))
                .AddCurrentHealth(new ReactiveVariable<float>(100))
                .AddIsDead()
                .AddInDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                .AddDeathProcessCurrentTime()
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddAttackProcessInitialTime(new ReactiveVariable<float>(3))
                .AddAttackProcessCurrentTime()
                .AddInAttackProcess()
                .AddStartAttackRequest()
                .AddStartAttackEvent()
                .AddEndAttackEvent()
                .AddAttackDelayTime(new ReactiveVariable<float>(1))
                .AddAttackDelayEndEvent()
                .AddInstantAttackDamage(new ReactiveVariable<float>(50))
                .AddAttackCanceledEvent()
                .AddAttackCooldownInitialTime(new ReactiveVariable<float>(2))
                .AddAttackCooldownCurrentTime()
                .AddInAttackCooldown();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false))
                .Add(new FuncCondition(() => entity.IsMoving.Value == false))
                .Add(new FuncCondition(() => entity.InAttackCooldown.Value == false));

            ICompositeCondition mustCancelAttack = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.IsMoving.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage)
                .AddCanStartAttack(canStartAttack)
                .AddMustCancelAttack(mustCancelAttack);

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem())
                .AddSystem(new AttackCancelSystem())
                .AddSystem(new StartAttackSystem())
                .AddSystem(new AttackProcessTimerSystem())
                .AddSystem(new AttackDelayEndTriggerSystem())
                .AddSystem(new InstantShootSystem(this))
                .AddSystem(new EndAttackSystem())
                .AddSystem(new AttackCooldownTimerSystem())
                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateHeroAlternative(Vector3 position)
        {
            Entity entity = CreateEmpty();
            MonoEntity monoEntity = _monoEntitiesFactory.Create(entity, position, "Entities/Characters/HeroAlternative");

            entity
                .AddMaxHealth(new ReactiveVariable<float>(100))
                .AddCurrentHealth(new ReactiveVariable<float>(100))
                .AddMaxEnergy(new ReactiveVariable<float>(100))
                .AddCurrentEnergy(new ReactiveVariable<float>(100))
                .AddEnergyRegenTickInterval(new ReactiveVariable<float>(5))
                .AddEnergyRegenCurrentTime(new ReactiveVariable<float>(5))
                .AddEnergyRegenValuePerTick(new ReactiveVariable<float>(15))
                .AddIsDead()
                .AddInDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                .AddDeathProcessCurrentTime()
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddExplosionStartRequest()
                .AddExplosionStartEvent()
                .AddExplosionEndEvent()
                .AddExplosionProcessInitialTime(new ReactiveVariable<float>(3))
                .AddExplosionProcessCurrentTime()
                .AddExplosionInProcess()
                .AddExplosionDelayTime(new ReactiveVariable<float>(1))
                .AddExplosionDelayEndEvent()
                .AddExplosionInstantDamage(new ReactiveVariable<float>(50))
                .AddExplosionRadius(new ReactiveVariable<float>(10))
                .AddExplosionEntitiesFilteredEvent()
                .AddAreaContactsCollidersBuffer(new Buffer<Collider>(64))
                .AddAreaContactsEntitiesBuffer(new Buffer<Entity>(64))
                .AddAreaContactDamage(new ReactiveVariable<float>(50))
                .AddAreaContactsDetectingMask(UnityLayersAPI.LayerMaskCharacters)
                .AddTeleportationStartEvent()
                .AddTeleportationEndEvent()
                .AddTeleportationStartRequest()
                .AddTeleportationInProcess()
                .AddTeleportationCost(new ReactiveVariable<float>(50))
                .AddTeleportationInitialTime(new ReactiveVariable<float>(2))
                .AddTeleportationCurrentTime()
                .AddTeleportationRadiusArea(new ReactiveVariable<float>(3));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canStartExplosion = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canStartTeleportation = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.TeleportationInProcess.Value == false))
                .Add(new FuncCondition(() => entity.CurrentEnergy.Value >= entity.TeleportationCost.Value));

            entity
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage)
                .AddTeleportationCanStart(canStartTeleportation)
                .AddExplosionCanStart(canStartExplosion);

            entity
                .AddSystem(new ExplosionStartSystem())
                .AddSystem(new ExplosionProcessTimerSystem())
                .AddSystem(new ExplosionDelayEndTriggerSystem())
                .AddSystem(new ExplosionEndSystem())
                .AddSystem(new ExplosionAreaDetectingSystem())
                .AddSystem(new ExplosionAreaEntitiesFilterSystem(_collidersRegistryService))
                .AddSystem(new DealDamageOnExplosionAreaSystem())
                .AddSystem(new TeleportExplosionAbilitySystem())
                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new EnergyRegenerationSystem())
                .AddSystem(new TeleportationStartSystem())
                .AddSystem(new TeleportationProcessTimerSystem())
                .AddSystem(new TeleportationEndSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateProjectile(Vector3 position, Vector3 direction, float damage)
        {
            Entity entity = CreateEmpty();
            MonoEntity monoEntity = _monoEntitiesFactory.Create(entity, position, "Entities/Projectile");

            entity
                .AddMoveDirection(new ReactiveVariable<Vector3>(direction))
                .AddRotationDirection(new ReactiveVariable<Vector3>(direction))
                .AddMoveSpeed(new ReactiveVariable<float>(10))
                .AddIsMoving()
                .AddRotationSpeed(new ReactiveVariable<float>(9999))
                .AddIsDead()
                .AddContactsDetectingMask(UnityLayersAPI.LayerMaskCharacters)
                .AddContactsCollidersBuffer(new Buffer<Collider>(64))
                .AddContactsEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(damage))
                .AddDeathMask(UnityLayersAPI.LayerMaskCharacters)
                .AddIsTouchDeathMask();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem())
                .AddSystem(new BodyContactsDetectingSystem())
                .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                .AddSystem(new DealDamageOnContactSystem())
                .AddSystem(new DeathMaskTouchDetectorSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new();
    }
}