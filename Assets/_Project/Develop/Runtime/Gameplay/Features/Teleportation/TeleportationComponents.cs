using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleportation
{
    public class TeleportationStartRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class TeleportationStartEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class TeleportationEndEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class TeleportationInProcess : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class TeleportationCanStart : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public class TeleportationCost : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportationInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportationCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportationRadiusArea : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}