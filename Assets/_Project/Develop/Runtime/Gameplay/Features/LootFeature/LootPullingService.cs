using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature
{
    public class LootPullingService : IInitializable, IDisposable
    {
        private readonly ReactiveVariable<bool> _allCollected = new();
        private readonly List<Entity> _loot = new();

        private EntitiesLifeContext _entitiesLifeContext;

        private bool _isActivated;

        public LootPullingService(EntitiesLifeContext entitiesLifeContext)
            => _entitiesLifeContext = entitiesLifeContext;

        public IReadOnlyVariable<bool> AllCollected => _allCollected;

        public void Initialize()
        {
            _entitiesLifeContext.Added += OnEntityAdded;
            _entitiesLifeContext.Removed += OnEntityRemoved;
        }

        public void Dispose()
        {
            _entitiesLifeContext.Added -= OnEntityAdded;
            _entitiesLifeContext.Removed -= OnEntityRemoved;
        }

        public void PullTo(Entity entity)
        {
            if (_isActivated)
                throw new InvalidOperationException();

            _isActivated = true;

            if (_loot.Count == 0)
            {
                _allCollected.Value = true;
                return;
            }

            foreach (Entity loot in _loot)
            {
                loot.CurrentTarget.Value = entity;
                loot.IsPullingProcess.Value = true;
            }

            Debug.Log("LOOT PULLTO");
        }

        public void Reset()
        {
            _isActivated = false;
            _allCollected.Value = false;
        }

        private void OnEntityAdded(Entity entity)
        {
            if (entity.HasComponent<IsPullable>() == false)
                return;

            _loot.Add(entity);

            Transform lootTransform = entity.Transform;

            Debug.Log("Loot ADDED");

            lootTransform
                .DOJump(GetRandomPositionAroundFor(lootTransform), 2, 1, 0.7f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => entity.SpawnInProcess.Value = false)
                .Play();
        }

        private void OnEntityRemoved(Entity entity)
        {
            bool lootRemoved = _loot.Remove(entity);

            Debug.Log("Loot REMOVED");

            if (lootRemoved && _loot.Count == 0)
                _allCollected.Value = true;
        }

        private Vector3 GetRandomPositionAroundFor(Transform transform)
        {
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector3 offset = new(randomOffset.x, 0, randomOffset.y);
            Vector3 endJumpPosition = transform.position + offset;

            return endJumpPosition;
        }
    }
}