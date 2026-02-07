using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Configs.StatsReset
{
    [CreateAssetMenu(menuName = "Configs/Meta/StatsReset/NewStatsResetConfig", fileName = "StatsResetConfig")]
    public class StatsResetConfig : ScriptableObject
    {
        [field: SerializeField] public int Price { get; private set; }
    }
}