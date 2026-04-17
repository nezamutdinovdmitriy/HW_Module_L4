using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup;
using Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public sealed class GameplayPopupService : PopupService
    {
        private GameplayUIRoot _root;
        private GameplayPresentersFactory _presentersFactory;

        public GameplayPopupService(
            ViewsFactory viewsFactory,
            ProjectPresentersFactory presentersFactory,
            GameplayUIRoot root,
            GameplayPresentersFactory gameplayPresentersFactory)
            : base(viewsFactory, presentersFactory)
        {
            _root = root;
            _presentersFactory = gameplayPresentersFactory;
        }

        protected override Transform PopupLayer => _root.PopupsLayer;

        public AbilitySelectPopupPresenter OpenAbilitySelectPopup(
            Entity entity,
            int level,
            Action closedCallback = null)
        {
            AbilitySelectPopupView view = _viewsFactory.Create<AbilitySelectPopupView>(ViewIDs.AbilitySelectPopup, PopupLayer);

            AbilitySelectPopupPresenter popup = _presentersFactory.CreateAbilitySelectPopupPresenter(view, entity, level);

            OnPopupCreated(popup, view, closedCallback);

            return popup;
        }

        public WinPopupPresenter OpenWinPopup(Action closedCallback = null)
        {
            WinPopupView view = _viewsFactory.Create<WinPopupView>(ViewIDs.WinPopup, PopupLayer);
            
            WinPopupPresenter popup = _presentersFactory.CreateWinPopupPresenter(view);

            OnPopupCreated(popup, view, closedCallback);

            return popup;
        }

        public DefeatPopupPresenter OpenDefeatPopup(Action closedCallback = null)
        {
            DefeatPopupView view = _viewsFactory.Create<DefeatPopupView>(ViewIDs.DefeatPopup, PopupLayer);

            DefeatPopupPresenter popup = _presentersFactory.CreateDefeatPopupPresenter(view);

            OnPopupCreated(popup, view, closedCallback);

            return popup;
        }
    }
}