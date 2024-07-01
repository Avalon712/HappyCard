using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace HappyCard
{
    public sealed class SavePlaybackCmd : ICommand
    {
        private GameFile _gameFile;

        public SavePlaybackCmd(GameFile gameFile)
        {
            _gameFile = gameFile;
        }

        public void Execute()
        {
            File.WriteAllText(Application.dataPath + "/Game Data/file.json", JsonConvert.SerializeObject(_gameFile));
        }
    }
}
