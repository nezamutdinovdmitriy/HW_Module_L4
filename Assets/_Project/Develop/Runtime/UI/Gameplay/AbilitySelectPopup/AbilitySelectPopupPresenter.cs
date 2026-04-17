using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using System;
using System.Collections.Generic;
using static Cinemachine.DocumentationSortingAttribute;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup
{
    public class AbilitySelectPopupPresenter : PopupPresenterBase
    {
        private const int AbilitiesCount = 2;

        private const string Tile = "LEVEL {0} IN THIS ADVENTURE";
        private const string SelectAbilityText = "Select Ability";

        private readonly AbilitySelectPopupView _view;

        private readonly Entity _entity;
        private readonly AbilityDropService _abilityDropper;
        private readonly GameplayPresentersFactory _presentersFactory;
        private readonly ViewsFactory _viewsFactory;

        private List<SelectableAbilityPresenter> _presenters = new();
        private SelectableAbilityPresenter _selectedPresenter;

        public AbilitySelectPopupPresenter(
            ICoroutinesPerformer coroutinesPerformer,
            AbilitySelectPopupView view,
            Entity entity,
            GameplayPresentersFactory presentersFactory,
            AbilityDropService abilityDropper,
            ViewsFactory viewsFactory)
            : base(coroutinesPerformer)
        {
            _view = view;
            _entity = entity;
            _abilityDropper = abilityDropper;
            _presentersFactory = presentersFactory;
            _viewsFactory = viewsFactory;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTitle(string.Format(Tile, _entity.Level.Value));
            _view.SetAdditionalText(SelectAbilityText);
            _view.SelectButtonOff();

            _view.SelectButtonClicked += OnSelectButtonClicked;

            List<AbilityConfig> dropOptions = _abilityDropper.Drop(AbilitiesCount, _entity);

            for (int i = 0; i < dropOptions.Count; i++)
            {
                SelectableAbilityView selectableAbilityView = _viewsFactory.Create<SelectableAbilityView>(ViewIDs.SelectableAbilityView);

                _view.AbilityListView.Add(selectableAbilityView);

                SelectableAbilityPresenter presenter = _presentersFactory
                    .CreateSelectableAbilityPresenter(dropOptions[i], selectableAbilityView, _entity);

                presenter.Selected += OnPresenterSelected;
                presenter.Initialize();

                _presenters.Add(presenter);
            }
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _view.SelectButtonOff();

            _view.SelectButtonClicked -= OnSelectButtonClicked;

            foreach (SelectableAbilityPresenter presenter in _presenters)
                presenter.Selected -= OnPresenterSelected;
        }

        public override void Dispose()
        {
            base.Dispose();

            _view.SelectButtonClicked -= OnSelectButtonClicked;

            foreach (SelectableAbilityPresenter presenter in _presenters)
            {
                presenter.Selected -= OnPresenterSelected;
                _view.AbilityListView.Remove(presenter.View);
                _viewsFactory.Release(presenter.View);
                presenter.Dispose();
            }

            _presenters.Clear();
        }

        private void OnPresenterSelected(SelectableAbilityPresenter presenter)
        {
            _view.SelectButtonOn();
            _view.AbilityListView.Select(presenter.View);
            _selectedPresenter = presenter;
        }

        private void OnSelectButtonClicked()
        {
            _selectedPresenter.Provide();
            OnCloseRequest();
        }
    }
}