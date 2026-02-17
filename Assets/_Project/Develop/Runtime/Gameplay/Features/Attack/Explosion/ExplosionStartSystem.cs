using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Optimization;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionStartSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _startRequest;
        private ReactiveEvent _startEvent;

        private ReactiveVariable<bool> _inProcess;

        private ICompositeCondition _startingCondition;

        private IDisposable _startRequestDisposable;

        public void OnInit(Entity entity)
        {
            _startRequest = entity.ExplosionStartRequest;
            _startEvent = entity.ExplosionStartEvent;

            _inProcess = entity.ExplosionInProcess;

            _startingCondition = entity.ExplosionCanStart;

            _startRequestDisposable = _startRequest.Subscribe(OnExplodeRequest);
        }

        private void OnExplodeRequest()
        {
            if (_startingCondition.Evaluate())
            {
                _inProcess.Value = true;
                _startEvent?.Invoke();

                Debug.Log("Started explosion process");
            }
            else
            {
                Debug.Log("Can't start explosion process");
            }
        }

        public void Dispose()
        {
            _startRequestDisposable.Dispose();
        }
    }

    public class ExplosionProcessTimerSystem : IInitializableSystem, IDisposable, IUpdatableSystem
    {
        private ReactiveEvent _startEvent;

        private ReactiveVariable<float> _processCurrentTime;
        private ReactiveVariable<bool> _inProcess;

        private IDisposable _startEventDisposable;

        public void OnInit(Entity entity)
        {
            _processCurrentTime = entity.ExplosionProcessCurrentTime;

            _inProcess = entity.ExplosionInProcess;

            _startEvent = entity.ExplosionStartEvent;

            _startEventDisposable = _startEvent.Subscribe(OnStartedExplosion);
        }

        public void Dispose()
        {
            _startEventDisposable.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inProcess.Value == false)
                return;

            _processCurrentTime.Value += deltaTime;
        }

        private void OnStartedExplosion()
        {
            _processCurrentTime.Value = 0;
        }
    }

    public class ExplosionDelayEndTriggerSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _explosionStartEvent;
        private ReactiveEvent _delayEndEvent;

        private ReactiveVariable<float> _delay;
        private ReactiveVariable<float> _processCurrentTime;

        private IDisposable _explosionStartEventDisposable;
        private IDisposable _processCurrentTimeDisposable;

        private bool _alreadyExploded;

        public void OnInit(Entity entity)
        {
            _explosionStartEvent = entity.ExplosionStartEvent;
            _delayEndEvent = entity.ExplosionDelayEndEvent;

            _delay = entity.ExplosionDelayTime;
            _processCurrentTime = entity.ExplosionProcessCurrentTime;

            _explosionStartEventDisposable = _explosionStartEvent.Subscribe(OnExplosionStarted);
            _processCurrentTimeDisposable = _processCurrentTime.Subscribe(OnCurrentTimeChanged);
        }

        public void Dispose()
        {
            _explosionStartEventDisposable.Dispose();
            _processCurrentTimeDisposable.Dispose();
        }

        private void OnExplosionStarted()
        {
            _alreadyExploded = false;
        }

        private void OnCurrentTimeChanged(float arg1, float currentTime)
        {
            if (_alreadyExploded)
                return;

            if (currentTime >= _delay.Value)
            {
                _delayEndEvent?.Invoke();
                _alreadyExploded = true;

                Debug.Log("The delay before explosion is over.");
            }
        }
    }

    public class ExplosionEndSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _endEvent;

        private ReactiveVariable<bool> _inProcess;

        private ReactiveVariable<float> _processInitialTime;
        private ReactiveVariable<float> _processCurrentTime;

        private IDisposable _processCurrentTimeDisposable;

        public void OnInit(Entity entity)
        {
            _endEvent = entity.ExplosionEndEvent;

            _inProcess = entity.ExplosionInProcess;

            _processInitialTime = entity.ExplosionProcessInitialTime;
            _processCurrentTime = entity.ExplosionProcessCurrentTime;

            _processCurrentTimeDisposable = _processCurrentTime.Subscribe(OnCurrentTimeChanged);
        }

        public void Dispose()
        {
            _processCurrentTimeDisposable.Dispose();
        }

        private void OnCurrentTimeChanged(float arg1, float currentTime)
        {
            if (TimerIsDone(currentTime))
            {
                _inProcess.Value = false;
                _endEvent?.Invoke();


                Debug.Log("Explosion ended");
            }
        }

        private bool TimerIsDone(float currentTime) => currentTime >= _processInitialTime.Value;
    }

    public class ExplosionAreaContactsDetectingSystem : IInitializableSystem, IDisposable
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

    public class ExplosionAreaEntitiesFilterSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _delayEndEvent;
        private ReactiveEvent _entitiesFilteredEvent;

        private Buffer<Collider> _contactsColliders;
        private Buffer<Entity> _contactsEntities;

        private readonly CollidersRegistryService _colllidersRegistryService;

        private IDisposable _delayEndEventDisposable;

        public ExplosionAreaEntitiesFilterSystem(CollidersRegistryService colllidersRegistryService)
        {
            _colllidersRegistryService = colllidersRegistryService;
        }

        public void OnInit(Entity entity)
        {
            _delayEndEvent = entity.ExplosionDelayEndEvent;
            _entitiesFilteredEvent = entity.ExplosionEntitiesFilteredEvent;

            _contactsColliders = entity.AreaContactsCollidersBuffer;
            _contactsEntities = entity.AreaContactsEntitiesBuffer;

            _delayEndEventDisposable = _delayEndEvent.Subscribe(OnFiltered);
        }

        public void Dispose()
        {
            _delayEndEventDisposable.Dispose();
        }

        public void OnFiltered()
        {
            _contactsEntities.Count = 0;

            for (int i = 0; i < _contactsColliders.Count; i++)
            {
                Collider collider = _contactsColliders.Items[i];

                Entity contactEntity = _colllidersRegistryService.GetBy(collider);

                if (contactEntity != null)
                {
                    _contactsEntities.Items[_contactsEntities.Count] = contactEntity;
                    _contactsEntities.Count++;
                }
            }

            _entitiesFilteredEvent?.Invoke();
        }
    }

    public class DealDamageOnExplosionAreaContactsSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _entitiesFilteredEvent;

        private Buffer<Entity> _contactsEntity;
        private ReactiveVariable<float> _damage;

        private IDisposable _entitiesFilteredEventDisposable;

        public void Dispose()
        {
            _entitiesFilteredEventDisposable.Dispose();
        }

        public void OnInit(Entity entity)
        {
            _entitiesFilteredEvent = entity.ExplosionEntitiesFilteredEvent;

            _contactsEntity = entity.AreaContactsEntitiesBuffer;

            _damage = entity.AreaContactDamage;

            _entitiesFilteredEventDisposable = _entitiesFilteredEvent.Subscribe(DealDamage);
        }

        public void DealDamage()
        {
            for (int i = 0; i < _contactsEntity.Count; i++)
            {
                Entity contactEntity = _contactsEntity.Items[i];

                if (contactEntity.HasComponent<TakeDamageRequest>())
                {
                    contactEntity.TakeDamageRequest.Invoke(_damage.Value);
                }
            }
        }
    }
}