public interface ILoadingScreen
{
    public bool IsShown { get; }
    public void Show();
    public void Hide();
}
