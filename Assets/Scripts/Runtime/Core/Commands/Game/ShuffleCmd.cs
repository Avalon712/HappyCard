using UnityEngine;

namespace HappyCard
{
    public sealed class ShuffleCmd : ICommand
    {
        public void Execute()
        {
            GameLoop loop = GameDataContainer.Instance.Loop;
            ShuffleMode mode = GameDataContainer.Instance.GameSetting.ShuffleMode;
            if (mode == ShuffleMode.Shuffle)
                loop.Rule.Shuffle(loop.Cards);
            else
                loop.Rule.NoShuffle(loop.Cards, Random.Range(20, 60));
        }
    }
}
