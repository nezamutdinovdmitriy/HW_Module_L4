using Assets._Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup
{
    public class SelectableAbilityView : MonoBehaviour, IShowableView
    {
        public event Action Clicked;

        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private Button _selectButton;

        [SerializeField] private AbilityIconView _icon;
        [SerializeField] private TMP_Text _tile;
        [SerializeField] private TMP_Text _description;

        [SerializeField] private TMP_Text _textOnTablet;

        [SerializeField] private Image _selectImage;

        private Sequence _currentAnimation;
        private float _startYOffset = 100;

        public AbilityIconView Icon => _icon;

        private void Awake() => _canvasGroup.alpha = 0;
        private void OnEnable() => _selectButton.onClick.AddListener(OnClicked);
        private void OnDisable() => _selectButton.onClick.RemoveListener(OnClicked);

        public void SetTile(string tile) => _tile.text = tile;
        public void SetDescription(string description) => _description.text = description;
        public void SetTabletText(string tabletText) => _textOnTablet.text = tabletText;

        public void Select() => _selectImage.gameObject.SetActive(true);
        public void Unselect() => _selectImage.gameObject.SetActive(false);

        private void OnClicked() => Clicked?.Invoke();

        public Tween Hide()
        {
            _currentAnimation?.Kill();

            _currentAnimation = DOTween.Sequence();

            return _currentAnimation
                .Append(_canvasGroup.DOFade(0, 0.4f))
                .Join(_canvasGroup.transform.DOLocalMoveY(_startYOffset, 0.4f))
                .SetUpdate(true)
                .Play();
        }

        public Tween Show()
        {
            _currentAnimation?.Kill();

            _currentAnimation = DOTween.Sequence();

            return _currentAnimation
                .Append(_canvasGroup.DOFade(1, 0.4f))
                .Join(_canvasGroup.transform.DOLocalMoveY(0, 0.4f).From(_startYOffset))
                .SetUpdate(true)
                .Play();
        }

        private void OnDestroy() => _currentAnimation?.Kill();
    }
}