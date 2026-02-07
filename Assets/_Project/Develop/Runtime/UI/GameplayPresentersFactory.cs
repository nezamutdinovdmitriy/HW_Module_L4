using Assets._Project.Develop.Runtime.Gameplay;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI.Gameplay;

public class GameplayPresentersFactory
{
    private readonly DIContainer _container;

    public GameplayPresentersFactory(DIContainer container)
    {
        _container = container;
    }

    public GameplaySequencePresenter CreateGameplaySequencePresenter(GameplayView view)
    {
        return new GameplaySequencePresenter(
            _container.Resolve<SequenceGameplay>(),
            view);
    }
}
