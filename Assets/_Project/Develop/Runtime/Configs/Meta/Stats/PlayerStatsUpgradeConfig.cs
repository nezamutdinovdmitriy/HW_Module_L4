using Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Meta.Stats
{
    [CreateAssetMenu(
        menuName = "Configs/Meta/Stats/NewPlayerStatsUpgradeConfig",
        fileName = "PlayerStatsUpgradeConfig")]
    public class PlayerStatsUpgradeConfig : ScriptableObject
    {
        [SerializeField] private List<StatUpgradeCostConfig> _stats = new();

        public StatUpgradeCostConfig GetStatConfig(StatType type)
            => _stats.First(stat => stat.Type == type);

        [Serializable]
        public class StatUpgradeCostConfig
        {
            [field: SerializeField] public StatType Type { get; private set; }
            [field: SerializeField] public List<float> StatValue { get; private set; }
            [field: SerializeField] public CurrencyTypes CostType { get; private set; } = CurrencyTypes.Gold;
            [field: SerializeField] public List<int> UpgradeToNextLevelCost { get; private set; }
        }
    }
}