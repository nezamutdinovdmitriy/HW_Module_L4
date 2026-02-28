using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly AIBrainsContext _brainsContext;
        private readonly IInputService _inputInputService;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public BrainsFactory(DIContainer container)
        {
            _container = container;

            _timerServiceFactory = container.Resolve<TimerServiceFactory>();
            _brainsContext = container.Resolve<AIBrainsContext>();
            _inputInputService = container.Resolve<IInputService>();
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
        }

        public StateMachineBrain CreateEnemyRandomTeleportationBrain(Entity entity)
        {
            List<IDisposable> disposables = new();

            TimerService timerBetweenTeleportation = _timerServiceFactory.Create(5f);

            AIStateMachine teleportation = new AIStateMachine(disposables);

            EmptyState empty = new();
            RandomTeleportationState randomTeleportationState = new(entity, 3f);

            disposables.Add(timerBetweenTeleportation);
            disposables.Add(randomTeleportationState.Entered.Subscribe(timerBetweenTeleportation.Restart));

            ICompositeCondition fromEmptyToTeleportationCondition = new CompositeCondition()
                .Add(new FuncCondition(() => timerBetweenTeleportation.IsOver))
                .Add(entity.TeleportationCanStart);

            ICompositeCondition fromTeleportationToEmptyCondition = new CompositeCondition()
                .Add(new FuncCondition(() => timerBetweenTeleportation.IsOver == false));

            teleportation.AddState(empty);
            teleportation.AddState(randomTeleportationState);

            teleportation.AddTransition(empty, randomTeleportationState, fromEmptyToTeleportationCondition);
            teleportation.AddTransition(randomTeleportationState, empty, fromTeleportationToEmptyCondition);

            StateMachineBrain brain = new(teleportation);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateMainHeroBrain(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);

            PlayerInputMovementState movementState = new(entity, _inputInputService);

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromMovementToCombatCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null))
                .Add(new FuncCondition(() => _inputInputService.Direciton == Vector3.zero));

            ICompositeCondition fromCombatToMovementCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => currentTarget.Value == null))
                .Add(new FuncCondition(() => _inputInputService.Direciton != Vector3.zero));

            AIStateMachine behaviour = new();

            behaviour.AddState(movementState);
            behaviour.AddState(combatState);

            behaviour.AddTransition(movementState, combatState, fromMovementToCombatCondition);
            behaviour.AddTransition(combatState, movementState, fromCombatToMovementCondition);

            FindTargetState findTargetState = new(targetSelector, _entitiesLifeContext, entity);

            AIParallelState parallelState = new(findTargetState, behaviour);

            AIStateMachine rootStateMachine = new();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new(rootStateMachine);
            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateGhostBrain(Entity entity)
        {
            AIStateMachine stateMachine = CreateRandomMovementStateMachine(entity);
            StateMachineBrain brain = new(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateRandomMovementStateMachine(Entity entity)
        {
            List<IDisposable> disposables = new();

            RandomMovementState randomMovementState = new(entity, 0.5f);

            EmptyState emptyState = new();

            TimerService movementTimer = _timerServiceFactory.Create(2f);
            disposables.Add(movementTimer);
            disposables.Add(randomMovementState.Entered.Subscribe(movementTimer.Restart));

            TimerService idleTimer = _timerServiceFactory.Create(3f);
            disposables.Add(idleTimer);
            disposables.Add(emptyState.Entered.Subscribe(idleTimer.Restart));

            FuncCondition movementTimerEndedCondition = new(() => movementTimer.IsOver);
            FuncCondition idleTimerEndedCondition = new(() => idleTimer.IsOver);

            AIStateMachine stateMachine = new(disposables);

            stateMachine.AddState(randomMovementState);
            stateMachine.AddState(emptyState);

            stateMachine.AddTransition(randomMovementState, emptyState, movementTimerEndedCondition);
            stateMachine.AddTransition(emptyState, randomMovementState, idleTimerEndedCondition);

            return stateMachine;
        }

        private AIStateMachine CreateAutoAttackStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new(entity);
            AttackTriggerState attackTriggerState = new(entity);

            ICondition canAttack = entity.CanStartAttack;

            Transform transform = entity.Transform;
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() =>
                {
                    Entity target = currentTarget.Value;

                    if (target == null)
                        return false;

                    float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Transform.position - transform.position));

                    return angleToTarget < 1f;
                }
                ));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICondition fromAttackToRotateCondition = new FuncCondition(() => inAttackProcess.Value == false);

            AIStateMachine stateMachine = new();

            stateMachine.AddState(rotateToTargetState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateCondition);

            return stateMachine;
        }
    }
}