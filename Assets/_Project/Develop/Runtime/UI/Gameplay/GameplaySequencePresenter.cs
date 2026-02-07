using Assets._Project.Develop.Runtime.Gameplay;
using Assets._Project.Develop.Runtime.UI.Core;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplaySequencePresenter : IPresenter
    {
        private SequenceGameplay _gameplay;
        private GameplayView _view;

        public GameplaySequencePresenter(SequenceGameplay gameplay, GameplayView view)
        {
            _gameplay = gameplay;
            _view = view;
        }

        public void Initialize()
        {
            _view.TargetSequence.SetText(_gameplay.TargetSequence);
            _view.InputSequence.SetText("");

            _gameplay.InputSequenceChanged += OnInputSequenceChanged;
        }

        public void Dispose()
        {
            _gameplay.InputSequenceChanged -= OnInputSequenceChanged;
        }

        private void OnInputSequenceChanged()
        {
            _view.InputSequence.SetText(_gameplay.InputBuffer.ToString());
        }
    }
}