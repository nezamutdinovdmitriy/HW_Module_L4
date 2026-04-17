using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature;
using Assets._Project.Develop.Runtime.UI.Core;
using System;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup
{
    public class SelectableAbilityPresenter : IPresenter
    {
        public event Action<SelectableAbilityPresenter> Selected;

        private AbilityFactory _abilityFactory;
        private Entity _entity;

        public SelectableAbilityPresenter(AbilityConfig abilityConfig, AbilityFactory abilityFactory, SelectableAbilityView view, Entity entity)
        {
            _abilityFactory = abilityFactory;
            _entity = entity;
            AbilityConfig = abilityConfig;
            View = view;
        }

        public AbilityConfig AbilityConfig { get; }
        public SelectableAbilityView View { get; }

        public void Initialize()
        {
            View.SetTile(AbilityConfig.Name);
            View.SetDescription(AbilityConfig.Discription);
            View.Icon.SetIcon(AbilityConfig.Icon);

            View.Icon.HideLevel();
            View.SetTabletText("NEW");

            View.Clicked += OnViewClicked;
        }

        public void Dispose() => View.Clicked -= OnViewClicked;

        public void Provide()
        {
            Ability ability = _abilityFactory.CreateAbilityFor(_entity, AbilityConfig);
            _entity.Abilities.Add(ability);
        }

        private void OnViewClicked() => Selected?.Invoke(this);

    }
}