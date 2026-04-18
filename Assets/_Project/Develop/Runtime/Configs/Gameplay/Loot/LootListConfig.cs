using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Loot
{
    [CreateAssetMenu(
        menuName = "Configs/Gameplay/Loot/NewLootListConfig",
        fileName = "LootListConfig")]
    public class LootListConfig : ScriptableObject
    {
        [SerializeField] private List<LootConfig> _lootConfigs;

        public IReadOnlyList<LootConfig> LootConfigs => _lootConfigs;

        public LootConfig GetLootFor(string id) => _lootConfigs.First(lootConfig => lootConfig.ID == id);
    }
}