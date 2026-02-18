using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;

        private EntitiesFactory _entitiesFactory;

        private bool _isRunning;

        private Entity _entity;

        public void Initialize(DIContainer container)
        {
            _container = container;

            _entitiesFactory = container.Resolve<EntitiesFactory>();
        }
        
        public void Run()
        {
            _entity = _entitiesFactory.CreateHeroAlternative(Vector3.zero);
            _entitiesFactory.CreateGhost(Vector3.zero + Vector3.forward * 5);
            _entitiesFactory.CreateGhost(Vector3.zero + Vector3.forward * 3);

            _isRunning = true;
        }

        public void Update()
        {
            if (_isRunning == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                _entity.TeleportationStartRequest.Invoke();

            //Vector3 input = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            
            //_entity.MoveDirection.Value = input;
            //_entity.RotationDirection.Value = input;
        }
    }
}