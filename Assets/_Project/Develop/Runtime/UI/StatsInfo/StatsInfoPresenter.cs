using Assets._Project.Develop.Runtime.Gameplay.GameplayServices;
using Assets._Project.Develop.Runtime.UI.CommonView;
using Assets._Project.Develop.Runtime.UI.Core;

namespace Assets._Project.Develop.Runtime.UI.StatsInfo
{
    public class StatsInfoPresenter : IPresenter
    {
        private readonly WinLossService _winLossService;
        private readonly TextView _view;

        public StatsInfoPresenter(WinLossService winLossService, TextView view)
        {
            _winLossService = winLossService;
            _view = view;
        }

        public void Initialize()
        {
            OnStatsChanged();

            _winLossService.StatsChanged += OnStatsChanged;
        }

        public void Dispose()
            => _winLossService.StatsChanged -= OnStatsChanged;

        private void OnStatsChanged()
            => _view.SetText(
                "Wins: " + _winLossService.TotalWins.ToString() +
                "\nLosses: " + _winLossService.TotalLosses.ToString());
    }
}