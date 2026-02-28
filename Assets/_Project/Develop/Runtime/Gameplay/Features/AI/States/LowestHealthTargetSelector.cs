using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class LowestHealthTargetSelector : ITargetSelector
    {
        private readonly Entity _source;

        public LowestHealthTargetSelector(Entity entity)
        {
            _source = entity;
        }

        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
        {
            IEnumerable<Entity> selectedTargets = targets.Where(target => target.HasComponent<CurrentHealth>() && target != _source);

            if (selectedTargets.Any() == false)
                return null;

            Entity lowestHealthTarget = selectedTargets.First();

            float minHealth = GetHealthTo(lowestHealthTarget);

            foreach (Entity target in selectedTargets)
            {
                float health = GetHealthTo(target);

                if (health < minHealth)
                {
                    minHealth = health;
                    lowestHealthTarget = target;
                }
            }

            return lowestHealthTarget;
        }

        private float GetHealthTo(Entity target) => target.CurrentHealth.Value;
    }
}