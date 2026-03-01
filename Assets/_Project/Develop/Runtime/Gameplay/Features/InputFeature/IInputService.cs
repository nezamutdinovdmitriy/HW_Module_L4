using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public interface IInputService
    {
        public bool IsEnabled { get; set; }

        public Vector3 MoveDireciton { get; }

        public Vector3? Aim { get; }

        public bool IsShooting { get; }
    }
}