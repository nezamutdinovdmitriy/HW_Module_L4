namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(GameModeType gameMode)
        {
            GameMode = gameMode;
        }

        public GameModeType GameMode { get; }
    }
}