using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPopupService : PopupService
    {
        private readonly MainMenuUIRoot _root;


        public MainMenuPopupService(
            ViewsFactory viewsFactory, 
            ProjectPresentersFactory presentersFactory,
            MainMenuUIRoot root)
            : base(viewsFactory, presentersFactory)
        {
            _root = root;
        }

        protected override Transform PopupLayer => _root.PopupsLayer;
    }
}