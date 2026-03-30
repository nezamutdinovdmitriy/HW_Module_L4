using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay
{
    public class EntityHealthPresenter : IPresenter
    {
        private readonly BarWithText _bar;
        private readonly Entity _entity;

        private ReactiveVariable<float> _currentHealth;
        private ReactiveVariable<float> _maxHealth;

        private ReactiveVariable<TeamType> _team;

        private List<IDisposable> _disposables = new();

        public EntityHealthPresenter(BarWithText bar, Entity entity)
        {
            _bar = bar;
            _entity = entity;
        }

        public BarWithText Bar => _bar;

        public void Initialize()
        {
            _currentHealth = _entity.CurrentHealth;
            _maxHealth = _entity.MaxHealth;
            _team = _entity.Team;

            _disposables.Add(_currentHealth.Subscribe(OnCurrentHealthChanged));
            _disposables.Add(_maxHealth.Subscribe(OnMaxHealthChanged));
            _disposables.Add(_team.Subscribe(OnTeamChanged));

            UpdateHealth();
            UpdateFillerColorBy(_team.Value);
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();
        }

        private void OnTeamChanged(TeamType oldValue, TeamType newValue) => UpdateFillerColorBy(newValue);

        private void OnMaxHealthChanged(float oldValue, float newValue) => UpdateHealth();

        private void OnCurrentHealthChanged(float oldValue, float newValue) => UpdateHealth();

        private void UpdateFillerColorBy(TeamType team)
        {
            if (team == TeamType.MainHero)
                _bar.SetFillerColor(Color.green);
            else if (team == TeamType.Enemies)
                _bar.SetFillerColor(Color.red);
        }

        private void UpdateHealth()
        {
            _bar.UpdateText(_currentHealth.Value.ToString("0"));
            _bar.UpdateValue(_currentHealth.Value / _maxHealth.Value);
        }
    }
}