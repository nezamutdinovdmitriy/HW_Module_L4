namespace Assets._Project.Develop.Runtime.Gameplay.Features.PauseFeature
{
    public interface IPauseService
    {
        public bool IsPaused { get; }
        public void Pause();
        public void Unpause();
    }
}