using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action OpenLevelsMenuButtonClicked;
        public event Action OpenUpgradeMenuButtonClicked;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }

        [SerializeField] private Button _openLevelsMenuButton;
        [SerializeField] private Button _openUpgradeMenuButton;

        private void OnEnable()
        {
            _openLevelsMenuButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);
            _openUpgradeMenuButton.onClick.AddListener(OnUpgradeMenuButtonClicked);
        }

        private void OnDisable()
        {
            _openLevelsMenuButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);
            _openUpgradeMenuButton.onClick.RemoveListener(OnUpgradeMenuButtonClicked);
        }

        private void OnOpenLevelsMenuButtonClicked() => OpenLevelsMenuButtonClicked?.Invoke();
        private void OnUpgradeMenuButtonClicked() => OpenUpgradeMenuButtonClicked?.Invoke();
    }
}
