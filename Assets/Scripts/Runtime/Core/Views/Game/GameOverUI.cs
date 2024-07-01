using System;
using System.Collections.Generic;
using UnityEngine;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;

namespace HappyCard
{
    /// <summary>
    /// 游戏结算UI
    /// </summary>
    public sealed class GameOverUI : EventRegister, ILoopListener, ICmdListener
    {
        private SelfResultUI _selfResultUI;
        private OthersResultUI _othersResultUI;
        private ResultBombUI _bombUI;
        private ResultPropUI _propUI;
        private Sprite _winIcon, _loseIcon;

        public GameOverUI(Sprite winIcon, Sprite loseIcon)
        {
            _selfResultUI = new SelfResultUI();
            _othersResultUI = new OthersResultUI();
            _bombUI = new ResultBombUI();
            _propUI = new ResultPropUI();
            _winIcon = winIcon;
            _loseIcon = loseIcon;
        }

        public void OnGameOver(GameFile file)
        {
            _bombUI.Clear();
            _propUI.Clear();

            ShuffleMode shuffleMode = GameDataContainer.Instance.GameSetting.ShuffleMode;
            switch (file.Gameplay)
            {
                case Gameplay.FightLandlord:
                    CmdExecutor.Instance.AddCmd(new FightLandlordSettlementCmd(shuffleMode, file.Winners, file.Losers, file.Bouts));
                    break;
                case Gameplay.BanZiPao:
                    break;
                case Gameplay.FriedGoldenFlower:
                    break;
            }

            Vue.Router.Open(nameof(GameUIs.GameOverView));
        }

        public void AfterCmdExecute(ICommand cmd)
        {
            if (cmd is FightLandlordSettlementCmd landlordCmd)
            {
                ShowSettlementResult_FightLandlord(ref landlordCmd);
            }
        }

        private void ShowSettlementResult_FightLandlord(ref FightLandlordSettlementCmd landlordCmd)
        {
            //显示当前玩家自己的结算结果
            int selfID = GameDataContainer.Instance.Self.ID;
            ValueTuple<int, int> incomeSelf = landlordCmd.Incomes[selfID];
            _selfResultUI.SetResult(incomeSelf.Item1, incomeSelf.Item2, landlordCmd.IsWinner(selfID) ? _winIcon : _loseIcon);

            //显示当前玩家自己本局的炸弹收益情况
            Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
            if (landlordCmd.BombRewards.TryGetValue(selfID, out List<ValueTuple<int, int, List<PokerCard>>> bombInfos))
            {
                for (int i = 0; i < bombInfos.Count; i++)
                {
                    List<PokerCard> pokerCards = bombInfos[i].Item3;
                    List<Sprite> cardIcons = new List<Sprite>(pokerCards.Count);
                    pokerCards.ForEach(card => cardIcons.Add(cardInfos[card].Icon));
                    _bombUI.AddBombInfo(bombInfos[i].Item2, bombInfos[i].Item2, cardIcons);
                }
            }

            //TODO: 显示道具收益情况

            //显示其它玩家的结果
            foreach (var playerID in landlordCmd.Incomes.Keys)
            {
                if (playerID == selfID) continue;
                ValueTuple<int, int> incomeOther = landlordCmd.Incomes[playerID];
                Sprite resultIcon = landlordCmd.IsWinner(playerID) ? _winIcon : _loseIcon;
                _othersResultUI.SetResult(landlordCmd.GetPlayer(playerID), incomeOther.Item1, incomeOther.Item2, resultIcon);
            }
        }


        #region UI事件


        [EventCall(nameof(ContinueGame))]
        private void ContinueGame()
        {
            GameDataContainer.Instance.Loop.GameState = GameState.ToBeStarted;
            Vue.Router.Close(nameof(GameUIs.GameOverView));
            _othersResultUI.ResetLikeState();
        }

        [EventCall(nameof(QuitRoom))]
        private void QuitRoom()
        {
            CmdExecutor.Instance.AddCmd(new QuitRoomCmd());
        }

        [EventCall(nameof(ReturnRoom))]
        private void ReturnRoom()
        {
            CmdExecutor.Instance.AddCmd(new ReturnRoomCmd());
        }

        [EventCall(nameof(Playback))]
        private void Playback()
        {
            CmdExecutor.Instance.AddCmd(new GamePlaybackCmd(GameDataContainer.Instance.Loop.File));
        }

        [EventCall(nameof(SavePlayback))]
        private void SavePlayback()
        {
            CmdExecutor.Instance.AddCmd(new SavePlaybackCmd(GameDataContainer.Instance.Loop.File));
        }

        #endregion


    }
}
