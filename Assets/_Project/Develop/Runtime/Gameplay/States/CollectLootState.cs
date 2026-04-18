using Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class CollectLootState : State, IUpdatableState
    {
        private LootPullingService _lootPullingService;
        private MainHeroHolderService _mainHeroHolderService;

        public CollectLootState(
            LootPullingService lootPullingService, 
            MainHeroHolderService mainHeroHolderService)
        {
            _lootPullingService = lootPullingService;
            _mainHeroHolderService = mainHeroHolderService;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("COLLECTION LOOT STATE");

            _lootPullingService.PullTo(_mainHeroHolderService.MainHero);
        }

        public void Update(float deltaTime)
        {
        }

        public override void Exit()
        {
            base.Exit();

            _lootPullingService.Reset();
        }
    }
}