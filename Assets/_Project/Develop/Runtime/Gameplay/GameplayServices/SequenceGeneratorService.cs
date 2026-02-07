using Assets._Project.Develop.Runtime.Gameplay.Configs;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.GameplayServices
{
    public class SequenceGeneratorService   
    {
        private readonly LevelConfig _config;
        private readonly int _length;

        public SequenceGeneratorService(LevelConfig config)
        {
            _config = config;
            _length = Random.Range(1, 6);
        }

        public LevelConfig Config => _config;

        public string Generate()
        {
            string result = "";

            for (int i = 0; i < _length; i++)
                result += _config.Symbols[Random.Range(0, _config.Symbols.Length)];

            return result;
        }
    }
}       