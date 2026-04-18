using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Loot
{
    [CreateAssetMenu(
        menuName = "Configs/Gameplay/Loot/NewCoinsLootConfig",
        fileName = "CoinsLootConfig")]
    public class CoinsLootConfig : LootConfig
    {
        [field: SerializeField] public int Coins { get; private set; }
    }
}