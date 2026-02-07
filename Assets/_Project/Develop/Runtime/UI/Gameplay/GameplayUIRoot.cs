using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayUIRoot : MonoBehaviour
    {
        [field: SerializeField] public Transform HUDLayer {  get; private set; }
        [field: SerializeField] public Transform VFXUnderPopupsLayerLayer {  get; private set; }
        [field: SerializeField] public Transform PopupsLayer {  get; private set; }
        [field: SerializeField] public Transform VFXOverPopupsLayer {  get; private set; }
    }
}