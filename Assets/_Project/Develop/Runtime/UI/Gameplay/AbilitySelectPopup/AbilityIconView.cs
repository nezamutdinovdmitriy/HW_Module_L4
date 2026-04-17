using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup
{
    public class AbilityIconView : MonoBehaviour, IView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Transform _levelParent;
        [SerializeField] private TMP_Text _level;

        public void HideLevel() => _level.gameObject.SetActive(false);
        public void ShowLevel() => _level.gameObject.SetActive(true);

        public void SetIcon(Sprite sprite) => _icon.sprite = sprite;

        public void SetLevel(string level) => _level.text = level;
    }
}