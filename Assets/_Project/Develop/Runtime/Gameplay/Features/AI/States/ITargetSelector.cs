using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Collections.Generic;

public interface ITargetSelector
{
    public Entity SelectTargetFrom(IEnumerable<Entity> targets);
}
