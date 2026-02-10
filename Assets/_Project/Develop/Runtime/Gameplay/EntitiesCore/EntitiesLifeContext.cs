using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System;
using System.Collections.Generic;

public class EntitiesLifeContext : IDisposable
{
    public event Action<Entity> Added;
    public event Action<Entity> Removed;

    private readonly List<Entity> _enities = new();
    private readonly List<Entity> _removeRequests = new();

    public void Add(Entity entity)
    {
        _enities.Add(entity);

        entity.Initialize();

        Added?.Invoke(entity);
    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < _enities.Count; i++)
            _enities[i].OnUpdate(deltaTime);

        foreach (Entity entity in _removeRequests)
        {
            _enities.Remove(entity);
            entity.Dispose();

            Removed?.Invoke(entity);
        }

        _removeRequests.Clear();
    }

    public void FixedUpdate(float deltaTime)
    {
        for (int i = 0; i < _enities.Count; i++)
            _enities[i].OnFixedUpdate(deltaTime);

        foreach (Entity entity in _removeRequests)
        {
            _enities.Remove(entity);
            entity.Dispose();

            Removed?.Invoke(entity);
        }

        _removeRequests.Clear();
    }

    public void Remove(Entity entity)
    {
        _removeRequests.Add(entity);
    }

    public void Dispose()
    {
        foreach (Entity entity in _enities)
            entity.Dispose();

        _enities.Clear();
        _removeRequests.Clear();
    }
}
