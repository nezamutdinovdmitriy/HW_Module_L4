using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.States;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private GameplayInputArgs _inputArgs;

        private GameplayStatesContext _gameplayStatesContext;
        private EntitiesLifeContext _entitiesLifeContext;
        private AIBrainsContext _brainsContext;

        private GameplayScreenPresenter _gameplayScreenPresenter;

        private MainHeroHolderService _mainHeroHolderService;

        private ResourcesAssetsLoader _resourcesAssetsLoader;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(_container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log($"Вы попали на уровень {_inputArgs.LevelNumber}");

            Debug.Log("Инициализация геймплейной сцены");

            _gameplayStatesContext = _container.Resolve<GameplayStatesContext>();
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _brainsContext = _container.Resolve<AIBrainsContext>();

            _gameplayScreenPresenter = _container.Resolve<GameplayScreenPresenter>();

            _container.Resolve<MainHeroFactory>().Create(Vector3.zero);

            _mainHeroHolderService = _container.Resolve<MainHeroHolderService>();

            _resourcesAssetsLoader = _container.Resolve<ResourcesAssetsLoader>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Старт геймплейной сцены");

            _resourcesAssetsLoader.Load<MonoEntity>("Entities/Projectile");

            _gameplayStatesContext.Run();
        }

        private void Update()
        {
            _brainsContext?.Update(Time.deltaTime);
            _entitiesLifeContext?.Update(Time.deltaTime);
            _gameplayStatesContext?.Update(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            }

            if (Input.GetKeyDown(KeyCode.Space))
                _mainHeroHolderService.MainHero.Experience.Value += 500;
        }

        private void FixedUpdate()
        {
            _entitiesLifeContext?.FixedUpdate(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            _gameplayScreenPresenter?.LateUpdate();
        }
    }
}
