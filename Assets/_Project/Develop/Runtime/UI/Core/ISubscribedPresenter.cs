using Assets._Project.Develop.Runtime.UI.Core;

public interface ISubscribedPresenter : IPresenter
{
    public void Subscribe();
    public void Unsubscribe();
}
