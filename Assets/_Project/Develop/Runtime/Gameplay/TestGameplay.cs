using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;

        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;

        private bool _isRunning;

        private Entity _entity;
        private Entity _ghost;

        public void Initialize(DIContainer container)
        {
            _container = container;

            _entitiesFactory = container.Resolve<EntitiesFactory>();
            _brainsFactory = container.Resolve<BrainsFactory>();

        }
        
        public void Run()
        {
            _entity = _entitiesFactory.CreateHero(Vector3.zero);
            _entity.AddCurrentTarget();
            _brainsFactory.CreateMainHeroBrain(_entity, new NearestDamageableTargetSelector(_entity));

            _ghost = _entitiesFactory.CreateGhost(Vector3.zero + Vector3.forward * 5);

            _isRunning = true;
        }

        public void Update()
        {
            if (_isRunning == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                _entity.TeleportationStartRequest.Invoke();

            if (Input.GetKeyDown(KeyCode.I))
                _brainsFactory.CreateGhostBrain(_ghost);
        }
    }
}