using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.Utilities.LoadingScreen
{
    public class StandardLoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private Slider _loadingProgressBar;
        public bool IsShown => gameObject.activeSelf;

        private void Awake()
        {
            Hide();
            DontDestroyOnLoad(this);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            _loadingProgressBar.value = 0;

            gameObject.SetActive(true);
        }
    }
}
