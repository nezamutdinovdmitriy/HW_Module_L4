using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature
{
    public class StatEffectsApplierSystem : IInitializableSystem, IDisposableSystem
    {
        private StatsEffectsList _statsEffects;

        private Dictionary<StatType, float> _baseStats;
        private Dictionary<StatType, float> _modifiedStats;

        public void OnInit(Entity entity)
        {
            _baseStats = entity.BaseStats;
            _modifiedStats = entity.ModifiedStats;

            _statsEffects = entity.StatsEffects;

            _statsEffects.Added += OnStatsEffectAdded;
            _statsEffects.Removed += OnStatsEffectRemoved;

            RecalculateStats();
        }

        public void OnDispose()
        {
            _statsEffects.Added -= OnStatsEffectAdded;
            _statsEffects.Removed -= OnStatsEffectRemoved;
        }

        private void RecalculateStats()
        {
            foreach (StatType stat in _baseStats.Keys)
                _modifiedStats[stat] = _baseStats[stat];

            foreach (IStatsEffect statsEffect in _statsEffects.Elements)
                statsEffect.ApplyTo(_modifiedStats);
        }

        private void OnStatsEffectRemoved(IStatsEffect effect) => RecalculateStats();

        private void OnStatsEffectAdded(IStatsEffect effect) => RecalculateStats();
    }
}