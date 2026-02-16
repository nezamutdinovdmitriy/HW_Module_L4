using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Explosion
{
    public class ExplosionRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CanStartExplosion : IEntityComponent
    {
        public ICompositeCondition Value;
    }
}