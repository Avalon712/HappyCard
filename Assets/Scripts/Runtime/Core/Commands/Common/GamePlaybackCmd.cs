namespace HappyCard
{

    public sealed class GamePlaybackCmd : ICommand
    {
        private GameFile _gameFile;

        public GamePlaybackCmd(GameFile gameFile)
        {
            _gameFile = gameFile;
        }

        public void Execute()
        {

        }
    }
}
