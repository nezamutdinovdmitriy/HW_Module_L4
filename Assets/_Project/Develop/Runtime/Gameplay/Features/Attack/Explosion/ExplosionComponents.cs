using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionStartRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class ExplosionStartEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class ExplosionEndEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class ExplosionCanStart : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public class ExplosionProcessInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionProcessCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionInProcess : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class ExplosionDelayTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionDelayEndEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class ExplosionInstantDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplosionEntitiesFilteredEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }
}