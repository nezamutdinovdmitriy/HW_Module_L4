using Assets._Project.Develop.Runtime.UI.Core.TestPopup;
using Assets._Project.Develop.Runtime.UI.LevelsMenuPopup;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public abstract class PopupService : IDisposable
    {
        protected readonly ViewsFactory _viewsFactory;

        private readonly ProjectPresentersFactory _presentersFactory;

        private readonly Dictionary<PopupPresenterBase, PopupInfo> _presenterToInfo = new();

        protected PopupService(
            ViewsFactory viewsFactory, 
            ProjectPresentersFactory presentersFactory)
        {
            _viewsFactory = viewsFactory;
            _presentersFactory = presentersFactory;
        }

        protected abstract Transform PopupLayer { get; }

        public TestPopupPresenter OpenTestPopup(Action closedCallback = null)
        {
            TestPopupView view = _viewsFactory.Create<TestPopupView>(ViewIDs.TestPopup, PopupLayer);

            TestPopupPresenter popup = _presentersFactory.CreateTestPopupPresenter(view);

            OnPopupCreated(popup, view, closedCallback);

            return popup;
        }

        public LevelsMenuPopupPresenter OpenLevelsMenuPopup()
        {
            LevelsMenuPopupView view = _viewsFactory.Create<LevelsMenuPopupView>(ViewIDs.LevelsMenuPopup, PopupLayer);

            LevelsMenuPopupPresenter popup = _presentersFactory.CreateLevelsMenuPopupPresenter(view);

            OnPopupCreated(popup, view);

            return popup;
        }

        public void ClosePopup(PopupPresenterBase popup)
        {
            popup.CloseRequest -= ClosePopup;

            popup.Hide(() =>
            {
                _presenterToInfo[popup].ClosedCallback?.Invoke();

                DisposeFor(popup);
                _presenterToInfo.Remove(popup);
            });
        }

        public void Dispose()
        {
            foreach (PopupPresenterBase popup in _presenterToInfo.Keys)
            {
                popup.CloseRequest -= ClosePopup;
                DisposeFor(popup);
            }

            _presenterToInfo.Clear();
        }

        protected void OnPopupCreated(
            PopupPresenterBase popup,
            PopupViewBase view,
            Action closedCallback = null)
        {
            PopupInfo popupInfo = new PopupInfo(view, closedCallback);

            _presenterToInfo.Add(popup, popupInfo);
            popup.Initialize();
            popup.Show();

            popup.CloseRequest += ClosePopup;
        }

        private void DisposeFor(PopupPresenterBase popup)
        {
            popup.Dispose();
            _viewsFactory.Release(_presenterToInfo[popup].View);
        }

        private class PopupInfo
        {
            public PopupInfo(PopupViewBase view, Action closedCallback)
            {
                View = view;
                ClosedCallback = closedCallback;
            }

            public PopupViewBase View { get; }
            public Action ClosedCallback { get; }
        }
    }
}
