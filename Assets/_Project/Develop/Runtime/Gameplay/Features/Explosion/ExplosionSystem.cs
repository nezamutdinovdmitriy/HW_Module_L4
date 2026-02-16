using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
    public class ExplosionSystem : IEntityComponent, IInitializableSystem

    {
        private ReactiveVariable<float> _radius;
        private IReadOnlyVariable<float> _damage;

        private ICompositeCondition _canStartExplosion;


        public void OnInit(Entity entity)
        {
            _radius = entity.ExplosionRadius;
            _damage = entity.ExplosionDamage;

            _canStartExplosion = entity.CanStartExplosion;
        }
    }
}