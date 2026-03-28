using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntity : MonoBehaviour
    {
        private CollidersRegistryService _colllidersRegistryService;
        private Entity _linkedEntity;

        public Entity LinkedEnitity => _linkedEntity;

        public void Initialize(CollidersRegistryService collidersRegistryService)
        {
            _colllidersRegistryService = collidersRegistryService;
        }

        public void Link(Entity entity)
        {
            _linkedEntity = entity;

            MonoEntityRegistrator[] registrators = GetComponentsInChildren<MonoEntityRegistrator>();

            if (registrators != null)
                foreach (MonoEntityRegistrator registrator in registrators)
                    registrator.Register(entity);

            MonoEntityView[] childrenViews = GetComponentsInChildren<MonoEntityView>();

            if (childrenViews != null)
                foreach (MonoEntityView childView in childrenViews)
                    childView.Link(entity);

            foreach (Collider collider in GetComponentsInChildren<Collider>())
                _colllidersRegistryService.Register(collider, entity);
        }

        public void Cleanup(Entity entity)
        {
            MonoEntityView[] childrenViews = GetComponentsInChildren<MonoEntityView>();

            if (childrenViews != null)
                foreach (MonoEntityView childView in childrenViews)
                    childView.Cleanup(entity);

            foreach (Collider collider in GetComponentsInChildren<Collider>())
                _colllidersRegistryService.Unregister(collider);

            _linkedEntity = null;
        }
    }
}