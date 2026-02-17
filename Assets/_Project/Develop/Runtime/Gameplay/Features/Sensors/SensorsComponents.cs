using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Optimization;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class BodyCollider : IEntityComponent
    {
        public CapsuleCollider Value;
    }

    public class ContactsDetectingMask : IEntityComponent
    {
        public LayerMask Value;
    }

    public class AreaContactsDetectingMask : IEntityComponent
    {
        public LayerMask Value;
    }

    public class ContactsCollidersBuffer : IEntityComponent
    {
        public Buffer<Collider> Value;
    }

    public class ContactsEntitiesBuffer : IEntityComponent
    {
        public Buffer<Entity> Value;
    }

    public class AreaContactsCollidersBuffer : IEntityComponent
    {
        public Buffer<Collider> Value;
    }

    public class AreaContactsEntitiesBuffer : IEntityComponent
    {
        public Buffer<Entity> Value;
    }

    public class DeathMask : IEntityComponent
    {
        public LayerMask Value;
    }

    public class IsTouchDeathMask : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }
}