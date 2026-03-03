using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesFactory
    {
        private readonly DIContainer _container;

        public GameplayStatesFactory(DIContainer container) => _container = container;

        public PreperationState CreatePreperationState() => new(_container.Resolve<PreperationTriggerService>());
        public StageProcessState CreateStageProcessState() => new(_container.Resolve<StageProviderService>());
        public WinState CreateWinState(GameplayInputArgs inputArgs) => new(
                _container.Resolve<IInputService>(),
                _container.Resolve<LevelsProgressionService>(),
                inputArgs,
                _container.Resolve<PlayerDataProvider>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>());
        
        public DefeatState CreateDefeatState() => new(
                _container.Resolve<IInputService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>());

        public GameplayStateMachine CreateGameplayStateMachine(GameplayInputArgs inputArgs)
        {
            PreperationTriggerService preperationTriggerService = _container.Resolve<PreperationTriggerService>();
            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();
            MainHeroHolderService mainHeroHolderService = _container.Resolve<MainHeroHolderService>();

            GameplayStateMachine coreLoopState = CreateCoreLoopState();
            DefeatState defeatState = CreateDefeatState();
            WinState winState = CreateWinState(inputArgs);

            ICompositeCondition fromCoreLoopToWinCondition = new CompositeCondition()
                .Add(new FuncCondition(() => preperationTriggerService.HasMainHeroContact.Value))
                .Add(new FuncCondition(() => stageProviderService.CurrentStageResult.Value == StageResult.Completed))
                .Add(new FuncCondition(() => stageProviderService.HasNextStage() == false));

            ICompositeCondition fromCoreLoopToDefeatCondition = new CompositeCondition()
                .Add(new FuncCondition(() =>
                {
                    if (mainHeroHolderService.MainHero != null)
                        return mainHeroHolderService.MainHero.IsDead.Value;

                    return false;
                }));

            GameplayStateMachine gameplayCycle = new();

            gameplayCycle.AddState(coreLoopState);
            gameplayCycle.AddState(winState);
            gameplayCycle.AddState(defeatState);

            gameplayCycle.AddTransition(coreLoopState, winState, fromCoreLoopToWinCondition);
            gameplayCycle.AddTransition(coreLoopState, defeatState, fromCoreLoopToDefeatCondition);

            return gameplayCycle;
        }

        public GameplayStateMachine CreateCoreLoopState()
        {
            PreperationTriggerService preperationTriggerService = _container.Resolve<PreperationTriggerService>();
            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();

            PreperationState preperationState = CreatePreperationState();
            StageProcessState stageProcessState = CreateStageProcessState();

            ICompositeCondition fromPreperationToStageProcessCondition = new CompositeCondition()
                .Add(new FuncCondition(() => preperationTriggerService.HasMainHeroContact.Value))
                .Add(new FuncCondition(() => stageProviderService.HasNextStage()));

            FuncCondition fromStageProcessToPreperationCondition = new(() 
                => stageProviderService.CurrentStageResult.Value == StageResult.Completed);

            GameplayStateMachine coreLoopState = new GameplayStateMachine();

            coreLoopState.AddState(preperationState);
            coreLoopState.AddState(stageProcessState);

            coreLoopState.AddTransition(preperationState, stageProcessState, fromPreperationToStageProcessCondition);
            coreLoopState.AddTransition(stageProcessState, preperationState, fromStageProcessToPreperationCondition);

            return coreLoopState;
        }
    }
}