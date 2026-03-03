using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PreperationState : State, IUpdatableState
    {
        private readonly PreperationTriggerService _triggerService;

        public PreperationState(PreperationTriggerService triggerService)
        {
            _triggerService = triggerService;
        }

        public override void Enter()
        {
            base.Enter();

            Vector3 nextStageTriggerPosition = Vector3.zero + Vector3.forward * 2;

            _triggerService.Create(nextStageTriggerPosition);
        }

        public void Update(float deltaTime)
        {
            _triggerService.Update(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();

            _triggerService.Cleanup();
        }
    }
}