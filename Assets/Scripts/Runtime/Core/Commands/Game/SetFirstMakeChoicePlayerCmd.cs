using UnityEngine;

namespace HappyCard
{
    public sealed class SetFirstMakeChoicePlayerCmd : ICommand
    {
        public void Execute()
        {
            GameLoop loop = GameDataContainer.Instance.Loop;

            switch (loop.File.Gameplay)
            {
                case Gameplay.FightLandlord:
                    SetFirstMakeChoicePlayer_FightLandlord(loop);
                    break;
                case Gameplay.BanZiPao:
                    SetFirstMakeChoicePlayer_BanZiPao(loop);
                    break;
                case Gameplay.FriedGoldenFlower:
                    break;
            }

            //发送同步
            NetworkManager.Instance.Service.SendBroadcast(nameof(SyncEvents.SetFirstMakeChoice), loop.VirtualPlayers[loop.CurrentIndex].Item1.ID);
        }

        private void SetFirstMakeChoicePlayer_FightLandlord(GameLoop loop)
        {
            //斗地主随机开始
            loop.SetFirstMakeChoicePlayer(Random.Range(0, loop.VirtualPlayers.Count - 1));
        }


        private void SetFirstMakeChoicePlayer_BanZiPao(GameLoop loop)
        {
            //十三张拥有黑桃7的玩家先开始
            for (int i = 0; i < loop.VirtualPlayers.Count; i++)
            {
                if (loop.Cards[i].Contains(PokerCard.Spade_7))
                {
                    loop.SetFirstMakeChoicePlayer(i);
                    return;
                }
            }
        }


        private void SetFirstMakeChoicePlayer_FriedGoldenFlower(GameLoop loop)
        {
            //TODO
        }
    }
}
