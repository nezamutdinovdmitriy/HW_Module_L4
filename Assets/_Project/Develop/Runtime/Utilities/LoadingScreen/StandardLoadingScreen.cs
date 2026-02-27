using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets._Project.Develop.Runtime.Utilities.LoadingScreen
{
    public class StandardLoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private Image _trobber;

        private Tween _loadingTween;

        public bool IsShown => gameObject.activeSelf;

        private void Awake()
        {
            Hide();
            DontDestroyOnLoad(this);
        }

        public void Hide()
        {
            if (_loadingTween != null)
            {
                _loadingTween.Kill();
                _loadingTween = null;
            }

            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (_loadingTween != null)
                _loadingTween.Kill();

            _loadingTween = _trobber.transform
                .DORotate(new Vector3(0, 0, -360), 5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .SetUpdate(true);

            _loadingTween.Play();
        }
    }
}
