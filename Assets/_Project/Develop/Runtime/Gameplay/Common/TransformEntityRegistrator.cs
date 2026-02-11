using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using UnityEngine;

public class TransformEntityRegistrator : MonoEntityRegistrator
{
    public override void Register(Entity entity)
    {
        entity.AddTransform(GetComponent<Transform>());
    }
}
