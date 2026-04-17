using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Discription { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}