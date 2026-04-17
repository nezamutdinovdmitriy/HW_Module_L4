using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.LevelUp
{
    [CreateAssetMenu(
        menuName = "Configs/Gameplay/LevelUp/NewExperienceForUpgradeLevelConfig",
        fileName = "ExperienceForUpgradeLevelConfig")]
    public class ExperienceForUpgradeLevelConfig : ScriptableObject
    {
        [SerializeField] private List<float> _experienceForLevel;

        public int MaxLevel => _experienceForLevel.Count;

        public float GetExperienceFor(int level) => _experienceForLevel[level - 1];
    }
}