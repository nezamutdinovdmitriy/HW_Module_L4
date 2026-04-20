using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot;
using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature.Abilities
{
    public class AdditionalDirectionsShotAbility : Ability, IDisposable
    {
        private AdditionalDirectionsShotAbilityConfig _config;
        private Entity _entity;

        private IDisposable _currentLevelChangedDisposable;

        public AdditionalDirectionsShotAbility(
            Entity entity,
            AdditionalDirectionsShotAbilityConfig config,
            int currentLevel)
            : base(config.ID, currentLevel, config.MaxLevel)
        {
            _config = config;
            _entity = entity;
        }

        public override void Activate()
        {
            for (int i = 0; i < CurrentLevel.Value; i++)
                AddShotDirectionsBy(i + 1);

            _currentLevelChangedDisposable = CurrentLevel.Subscribe(OnCurrentLevelChanged);
        }

        private void OnCurrentLevelChanged(int previousLevel, int newLevel)
        {
            for (int i = previousLevel; i < newLevel; i++)
                AddShotDirectionsBy(i + 1);
        }

        private void AddShotDirectionsBy(int level)
        {
            List<DirectionShotConfig> directionShotConfigs = _config.GetBy(level);

            InstantShootingDirectionArgs shootingArgs = _entity.InstantShootingDirections;

            foreach (DirectionShotConfig directionShotConfig in directionShotConfigs)
                shootingArgs.Add(
                    new InstantShotDirectionArgs(directionShotConfig.Angel, directionShotConfig.NumberOfProjectiles));
        }

        public void Dispose() => _currentLevelChangedDisposable?.Dispose();
    }
}