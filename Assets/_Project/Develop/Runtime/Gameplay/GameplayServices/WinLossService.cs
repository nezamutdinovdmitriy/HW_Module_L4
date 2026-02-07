using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using System;
namespace Assets._Project.Develop.Runtime.Gameplay.GameplayServices
{
    public class WinLossService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        public event Action StatsChanged;

        private int _totalWins;
        private int _totalLosses;

        private readonly PlayerDataProvider _playerDataProvider;

        public WinLossService(PlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;

            _playerDataProvider.RegisterWriter(this);
            _playerDataProvider.RegisterReader(this);
        }

        public int TotalWins => _totalWins;
        public int TotalLosses => _totalLosses;

        public void AddWins()
        {
            _totalWins++;
            StatsChanged?.Invoke();
        }
        public void AddLosses()
        {
            _totalLosses++;
            StatsChanged?.Invoke();
        }

        public void Reset()
        {
            _totalWins = 0;
            _totalLosses = 0;
            StatsChanged?.Invoke();
        }

        public void ReadFrom(PlayerData data)
        {
            _totalWins = data.TotalWins;
            _totalLosses = data.TotalLosses;
        }

        public void WriteTo(PlayerData data)
        {
            data.TotalWins = _totalWins;
            data.TotalLosses = _totalLosses;
        }
    }
}