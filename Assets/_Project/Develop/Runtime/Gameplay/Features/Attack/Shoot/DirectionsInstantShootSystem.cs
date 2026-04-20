using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class DirectionsInstantShootSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly EntitiesFactory _entitiesFactory;

        private InstantShootingDirectionArgs _directions;
        private ReactiveEvent _attackDelayEndEvent;

        private Entity _entity;

        private ReactiveVariable<float> _damage;
        private Transform _shootPoint;

        private IDisposable _attackDelayEndDisposable;

        public DirectionsInstantShootSystem(EntitiesFactory entitiesFactory)
            => _entitiesFactory = entitiesFactory;

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _attackDelayEndEvent = entity.AttackDelayEndEvent;
            _directions = entity.InstantShootingDirections;


            _damage = entity.InstantAttackDamage;
            _shootPoint = entity.ShootPoint;

            _attackDelayEndDisposable = _attackDelayEndEvent.Subscribe(OnAttackDelayEnd);
        }

        public void OnDispose() => _attackDelayEndDisposable?.Dispose();

        private void OnAttackDelayEnd()
        {
            foreach (var arg in _directions.Args)
            {
                Shoot(arg.Angel, arg.ProjectileCounts);
            }
        }

        private void Shoot(int angel, int projectileCounts)
        {
            Vector3 directionForShoot = Quaternion.Euler(new(0, angel, 0)) * _shootPoint.forward;
            Vector2 perpendicular = Vector2.Perpendicular(new(directionForShoot.x, directionForShoot.z)).normalized;

            float offsetBetweenProjectiles = 0.6f;

            for (int i = 0; i < projectileCounts; i++)
            {
                Vector2 offset = perpendicular * (-offsetBetweenProjectiles / 2f * (projectileCounts - 1) + i * offsetBetweenProjectiles);
                Vector3 position = new(_shootPoint.position.x + offset.x, _shootPoint.position.y, _shootPoint.position.z + offset.y);

                _entitiesFactory.CreateProjectile(position, directionForShoot, _damage.Value, _entity);
            }
        }
    }
}