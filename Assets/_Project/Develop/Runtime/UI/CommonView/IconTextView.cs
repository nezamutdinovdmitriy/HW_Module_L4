using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.CommonView
{
    public class IconTextView : MonoBehaviour, IView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;

        public void SetIcon(Sprite image) => _image.sprite = image;

        public void SetText(string text) => _text.text = text;
    }
}