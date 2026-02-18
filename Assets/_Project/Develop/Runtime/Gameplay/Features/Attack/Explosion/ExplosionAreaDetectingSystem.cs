using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Optimization;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionAreaDetectingSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _delayEndEvent;

        private ReactiveVariable<float> _radius;

        private Buffer<Collider> _contacts;
        private LayerMask _layerMask;

        private CapsuleCollider _body;

        private IDisposable _delayEndEventDisposable;

        public void OnInit(Entity entity)
        {
            _delayEndEvent = entity.ExplosionDelayEndEvent;

            _radius = entity.ExplosionRadius;

            _contacts = entity.AreaContactsCollidersBuffer;
            _layerMask = entity.AreaContactsDetectingMask;

            _body = entity.BodyCollider;

            _delayEndEventDisposable = _delayEndEvent.Subscribe(OnExploded);
        }

        public void Dispose()
        {
            _delayEndEventDisposable.Dispose();
        }

        private void OnExploded()
        {
            _contacts.Count = Physics.OverlapSphereNonAlloc(
                _body.transform.position,
                _radius.Value,
                _contacts.Items,
                _layerMask,
                QueryTriggerInteraction.Ignore);

            RemoveSelfFromContacts();
        }

        private void RemoveSelfFromContacts()
        {
            int indexToRemove = -1;

            for (int i = 0; i < _contacts.Count; i++)
            {
                if (_contacts.Items[i] == _body)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove >= 0)
            {
                for (int i = indexToRemove; i < _contacts.Count - 1; i++)
                {
                    _contacts.Items[i] = _contacts.Items[i + 1];
                }

                _contacts.Count--;
            }
        }
    }
}