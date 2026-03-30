using Assets._Project.Develop.Runtime.UI.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class DefeatPopupView : PopupViewBase
    {
        public event Action RestartClicked;
        public event Action ExitClicked;

        [SerializeField] private TMP_Text _tile;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        public void SetTile(string tile) => _tile.text = tile;

        protected override void OnPostShow()
        {
            base.OnPostShow();

            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked() => RestartClicked?.Invoke();

        private void OnExitButtonClicked() => ExitClicked?.Invoke();
    }
}