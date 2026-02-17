using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ExplosionTakeDamage
{
    public class ExplosionAreaRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionAreaDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionAreaEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }
}