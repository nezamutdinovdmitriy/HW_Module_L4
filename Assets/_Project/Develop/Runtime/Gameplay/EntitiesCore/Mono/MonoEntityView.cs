using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public abstract class MonoEntityView : MonoBehaviour
    {
        public void Link(Entity entity) => entity.Initialized += OnEntityInitialized;

        public virtual void Cleanup(Entity entity) => entity.Initialized -= OnEntityInitialized;

        protected abstract void OnEntityInitialized(Entity entity);
    }
}