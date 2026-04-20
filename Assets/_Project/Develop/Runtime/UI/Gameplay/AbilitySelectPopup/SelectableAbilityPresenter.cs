using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using System.Linq;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup
{
    public class SelectableAbilityPresenter : IPresenter
    {
        public event Action<SelectableAbilityPresenter> Selected;

        private AbilityFactory _abilityFactory;
        private Entity _entity;

        private int _level;

        public SelectableAbilityPresenter(
            AbilityConfig abilityConfig,
            AbilityFactory abilityFactory,
            SelectableAbilityView view, 
            Entity entity,
            int level)
        {
            _abilityFactory = abilityFactory;
            _entity = entity;
            AbilityConfig = abilityConfig;
            View = view;
            _level = level;
        }

        public AbilityConfig AbilityConfig { get; }
        public SelectableAbilityView View { get; }

        public void Initialize()
        {
            View.SetTile(AbilityConfig.Name);
            View.SetDescription(AbilityConfig.Discription);
            View.Icon.SetIcon(AbilityConfig.Icon);

            InitByAbilityConfig();

            View.Clicked += OnViewClicked;
        }

        public void Dispose() => View.Clicked -= OnViewClicked;

        public void Provide()
        {
            Ability ability;

            if (AbilityConfig.IsUpgradable())
            {
                ability = _entity.Abilities.Elements.FirstOrDefault(ability => ability.ID == AbilityConfig.ID);

                if(ability != null)
                {
                    ability.AddLevel(_level);
                    return;
                }
            }

            ability = _abilityFactory.CreateAbilityFor(_entity, AbilityConfig, _level);
            _entity.Abilities.Add(ability);
        }

        private void InitByAbilityConfig()
        {
            if (AbilityConfig.IsUpgradable())
            {
                Ability ability = _entity.Abilities.Elements.FirstOrDefault(ability => ability.ID == AbilityConfig.ID);

                if (ability != null)
                {
                    View.Icon.ShowLevel();
                    View.Icon.SetLevel($"LV.{ability.CurrentLevel.Value}");
                    View.SetTabletText($"LV.{ability.CurrentLevel.Value} -> LV.{ability.CurrentLevel.Value + _level}");
                }
                else
                {
                    View.Icon.HideLevel();
                    View.SetTabletText($"NEW LV.{_level}");
                }
            }
            else
            {
                View.Icon.HideLevel();
                View.SetTabletText($"NEW");
            }
        }

        private void OnViewClicked() => Selected?.Invoke(this);

    }
}