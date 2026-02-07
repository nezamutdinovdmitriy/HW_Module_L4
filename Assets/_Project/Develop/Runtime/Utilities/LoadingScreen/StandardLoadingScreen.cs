using UnityEngine;

public class StandardLoadingScreen : MonoBehaviour, ILoadingScreen
{
    public bool IsShown => gameObject.activeSelf;

    private void Awake()
    {
        Hide();
        DontDestroyOnLoad(this);
    }

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
