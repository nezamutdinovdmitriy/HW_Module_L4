using Assets._Project.Develop.Runtime.UI.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.StatsUpgradePopup
{
    public class BuyButtonView : MonoBehaviour, IView
    {
        public event Action Clicked;

        [SerializeField] private Button _button;

        [SerializeField] private Image _background;

        [SerializeField] private Sprite _availableSprite;
        [SerializeField] private Sprite _lockedSprite;

        [Space]

        [SerializeField] private Image _priceIcon;
        [SerializeField] private TMP_Text _priceText;

        private void OnEnable() => _button.onClick.AddListener(OnClicked);

        private void OnDisable() => _button.onClick.RemoveListener(OnClicked);

        public virtual void Lock() => _background.sprite = _lockedSprite;
        public virtual void Unlock() => _background.sprite = _availableSprite;

        public void SetIcon(Sprite sprite) => _priceIcon.sprite = sprite;
        public void SetPriceText(string priceText) => _priceText.text = priceText;

        public void HideIcon() => _priceIcon.gameObject.SetActive(false);
        public void ShowIcon() => _priceIcon.gameObject.SetActive(true);

        private void OnClicked() => Clicked?.Invoke();
    }
}