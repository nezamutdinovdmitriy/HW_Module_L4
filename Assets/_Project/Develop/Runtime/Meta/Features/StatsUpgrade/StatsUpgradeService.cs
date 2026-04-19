using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using static Assets._Project.Develop.Runtime.Configs.Meta.Stats.PlayerStatsUpgradeConfig;

namespace Assets._Project.Develop.Runtime.Meta.Features.StatsUpgrade
{
    public class StatsUpgradeService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly ConfigsProviderService _configsProvider;
        private Dictionary<StatType, ReactiveVariable<int>> _statLevels = new();

        public StatsUpgradeService(PlayerDataProvider dataProvider, ConfigsProviderService configsProvider)
        {
            _configsProvider = configsProvider;

            dataProvider.RegisterReader(this);
            dataProvider.RegisterWriter(this);
        }

        public List<StatType> AvailableStats => _statLevels.Keys.ToList();

        public IReadOnlyVariable<int> GetStatLevelFor(StatType statType)
            => _statLevels[statType];

        private PlayerStatsUpgradeConfig PlayerStatsByLevelConfig => _configsProvider.GetConfig<PlayerStatsUpgradeConfig>();

        public float GetCurrentStatValueFor(StatType type)
            => PlayerStatsByLevelConfig.GetStatConfig(type).StatValue[_statLevels[type].Value - 1];

        public CurrencyTypes GetUpgradeCostTypeFor(StatType type)
            => PlayerStatsByLevelConfig.GetStatConfig(type).CostType;

        public bool TryGetStatValueForNextLevel(StatType type, out float statValue)
        {
            StatUpgradeCostConfig statData = PlayerStatsByLevelConfig.GetStatConfig(type);

            if(statData.StatValue.Count <= _statLevels[type].Value)
            {
                statValue = 0;
                return false;
            }

            statValue = statData.StatValue[_statLevels[type].Value];
            return true;
        }

        public bool TryGetUpgradeCostFor(StatType type, out CurrencyTypes costType, out int cost)
        {
            StatUpgradeCostConfig statData = PlayerStatsByLevelConfig.GetStatConfig(type);

            if (statData.UpgradeToNextLevelCost.Count <= _statLevels[type].Value - 1)
            {
                costType = default;
                cost = 0;
                return false;
            }

            costType = statData.CostType;
            cost = statData.UpgradeToNextLevelCost[_statLevels[type].Value - 1];
            return true;
        }

        public bool TryUpgradeStat(StatType type)
        {
            StatUpgradeCostConfig statData = PlayerStatsByLevelConfig.GetStatConfig(type);

            if (statData.StatValue.Count <= _statLevels[type].Value)
                return false;

            _statLevels[type].Value += 1;
            return true;
        }

        public void ReadFrom(PlayerData data)
        {
            foreach (var statLevel in data.StatsUpgradeLevel)
            {
                if (_statLevels.ContainsKey(statLevel.Key))
                    _statLevels[statLevel.Key].Value = statLevel.Value;
                else
                    _statLevels.Add(statLevel.Key, new ReactiveVariable<int>(statLevel.Value));
            }
        }

        public void WriteTo(PlayerData data)
        {
            foreach (var stat in _statLevels)
            {
                if (data.StatsUpgradeLevel.ContainsKey(stat.Key))
                    data.StatsUpgradeLevel[stat.Key] = stat.Value.Value;
                else
                    data.StatsUpgradeLevel.Add(stat.Key, stat.Value.Value);
            }
        }
    }
}