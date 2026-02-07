using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/LevelsConfigs", fileName = "LevelsConfigs")]
    public class LevelsListConfigs : ScriptableObject
    {
        [SerializeField] private List<Configs> _levels;

        public IReadOnlyList<Configs> Levels => _levels;

        public LevelConfig GetLevelConfigBy(GameModeType gameMode) => _levels.First(config => config.GameMode == gameMode).LevelConfig;

        [Serializable]
        public class Configs
        {
            [field: SerializeField] public GameModeType GameMode {  get; private set; }
            [field: SerializeField] public LevelConfig LevelConfig { get; private set; }
        }
    }
}