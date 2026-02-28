using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

public class EnergySpendSystem : IInitializableSystem, ISpendable
{
    private ReactiveVariable<float> _currentEnergy;

    public void OnInit(Entity entity)
    {
        _currentEnergy = entity.CurrentEnergy;
    }

    public bool CanSpend()
    {
        throw new System.NotImplementedException();
    }

    public void Spend()
    {
        throw new System.NotImplementedException();
    }
}
