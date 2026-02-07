using Assets._Project.Develop.Runtime.UI.CommonView;
using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

public class GameplayView : MonoBehaviour, IView
{
    [field: SerializeField] public TextView TargetSequence { get; private set; }
    [field: SerializeField] public TextView InputSequence { get; private set; }
}
