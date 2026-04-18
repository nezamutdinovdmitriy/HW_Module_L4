using Assets._Project.Develop.Runtime.Configs.Gameplay.Loot;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature
{
    public class DropLootService
    {
        private LootListConfig _lootListConfigs;
        private LootFactory _lootFactory;

        public DropLootService(
            LootListConfig lootListConfigs,
            LootFactory lootFactory)
        {
            _lootListConfigs = lootListConfigs;
            _lootFactory = lootFactory;
        }

        public void DropLootFor(Entity entity)
        {
            Transform entityTransform = entity.Transform;

            List<ExperienceLootConfig> experienceConfigs = _lootListConfigs.LootConfigs
                .Where(loot => loot.GetType() == typeof(ExperienceLootConfig))
                .Cast<ExperienceLootConfig>()
                .ToList();

            if (experienceConfigs.Count > 0)
                DropExperience(entityTransform.position, experienceConfigs[Random.Range(0, experienceConfigs.Count)]);

            DropCoins(entityTransform.position);
            DropHealth(entityTransform.position);
        }

        public void DropExperience(Vector3 position, ExperienceLootConfig config)
        {
            int experienceInOnePotion = 300;

            if (config.Experience < experienceInOnePotion)
            {
                _lootFactory.CreateExperienceLoot(config.PrefabPath, position, config.Experience);
            }
            else
            {
                int restOfExperienct = config.Experience % experienceInOnePotion;

                int potionNumber = (config.Experience - restOfExperienct) / experienceInOnePotion;

                for (int i = 0; i < potionNumber; i++)
                    _lootFactory.CreateExperienceLoot(config.PrefabPath, position, experienceInOnePotion);
            }
        }

        public void DropCoins(Vector3 position)
        {
            List<CoinsLootConfig> coinsConfigs = _lootListConfigs.LootConfigs
                .Where(config => config.GetType() == typeof(CoinsLootConfig))
                .Cast<CoinsLootConfig>()
                .ToList();

            if(coinsConfigs.Count > 0 && Random.Range(0, 100) > 50)
            {
                CoinsLootConfig config = coinsConfigs[Random.Range(0, coinsConfigs.Count)];

                _lootFactory.CreateCoinsLoot(config.PrefabPath, position, config.Coins);
            }
        }

        public void DropHealth(Vector3 position)
        {
            List<HealthLootConfig> healthConfigs = _lootListConfigs.LootConfigs
                .Where(config => config.GetType() == typeof(HealthLootConfig))
                .Cast<HealthLootConfig>()
                .ToList();

            if (healthConfigs.Count > 0 && Random.Range(0, 100) > 50)
            {
                HealthLootConfig config = healthConfigs[Random.Range(0, healthConfigs.Count)];

                _lootFactory.CreateHealthLoot(config.PrefabPath, position, config.Health);
            }
        }
    }
}