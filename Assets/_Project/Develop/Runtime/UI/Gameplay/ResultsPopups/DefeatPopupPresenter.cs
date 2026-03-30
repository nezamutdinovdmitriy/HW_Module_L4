using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class DefeatPopupPresenter : PopupPresenterBase
    {
        private const string TileName = "YOU LOOSE!";

        private readonly DefeatPopupView _view;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameplayInputArgs _currentLevelArgs;

        public DefeatPopupPresenter(
            ICoroutinesPerformer coroutinesPerformer, DefeatPopupView view, SceneSwitcherService sceneSwitcherService, GameplayInputArgs gameplayInputArgs)
            : base(coroutinesPerformer)
        {
            _coroutinesPerformer = coroutinesPerformer;
            _view = view;
            _sceneSwitcher = sceneSwitcherService;
            _currentLevelArgs = gameplayInputArgs;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTile(TileName);

            _view.ExitClicked += OnExitClicked;
            _view.RestartClicked += OnRestartClicked;
        }

        public override void Dispose()
        {
            base.Dispose();

            _view.ExitClicked += OnExitClicked;
            _view.RestartClicked += OnRestartClicked;
        }

        private void OnRestartClicked()
        {
            _coroutinesPerformer.StartPerform(_sceneSwitcher.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(_currentLevelArgs.LevelNumber)));
            OnCloseRequest();
        }

        private void OnExitClicked()
        {
            _coroutinesPerformer.StartPerform(_sceneSwitcher.ProcessSwitchTo(Scenes.MainMenu));
            OnCloseRequest();
        }
    }
}