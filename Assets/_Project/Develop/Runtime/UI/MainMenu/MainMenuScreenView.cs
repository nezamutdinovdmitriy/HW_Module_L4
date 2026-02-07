using Assets._Project.Develop.Runtime.UI.CommonView;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action OpenLevelsMenuButtonClicked;
        public event Action PurchaseResetStatsButtonClicked;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }

        [SerializeField] private Button _openLevelsMenuButton;
        [SerializeField] private Button _purchaseResetStatsButton;

        private void OnEnable()
        {
            _openLevelsMenuButton.onClick.AddListener(OnLevelsMenuButtonClicked);
            _purchaseResetStatsButton.onClick.AddListener(OnPurchaseResetStatsButtonClicked);
        }

        private void OnDisable()
        {
            _openLevelsMenuButton.onClick.RemoveListener(OnLevelsMenuButtonClicked);
            _purchaseResetStatsButton.onClick.RemoveListener(OnPurchaseResetStatsButtonClicked);
        }

        private void OnLevelsMenuButtonClicked() => OpenLevelsMenuButtonClicked?.Invoke();
        private void OnPurchaseResetStatsButtonClicked() => PurchaseResetStatsButtonClicked?.Invoke();
    }
}