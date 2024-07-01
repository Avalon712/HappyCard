namespace HappyCard
{
    public sealed class GameTopInfoUI : ILoopListener, ICmdListener
    {
        private RecordCardsUI _recordCardsUI;           //记牌器
        private ExtraCardInfoUI _extraCardInfoUI;       //显示额外的牌的信息

        public GameTopInfoUI()
        {
            _recordCardsUI = new RecordCardsUI();
            _extraCardInfoUI = new ExtraCardInfoUI();
        }

        /// <summary>
        /// 命令执行后
        /// </summary>
        public void AfterCmdExecute(ICommand cmd)
        {
            if (cmd is CallLandlordCmd)
            {
                _extraCardInfoUI.ShowLandlordCards();
                GameDataContainer container = GameDataContainer.Instance;
                //记牌器只记录除开玩家手牌的牌
                _recordCardsUI.ShowResidualCards(container.Loop.GetCards(container.Self.ID));
            }
        }


        public void OnNext(Bout bout)
        {
            //由于当前玩家自己的手牌已经排除了，因此如果是当前玩家自己出的牌则无需再进行记录
            if (bout.PlayerID != GameDataContainer.Instance.Self.ID)
                _recordCardsUI.ShowResidualCards(bout.OutCards);
        }


        public void OnGameOver(GameFile file)
        {
            _extraCardInfoUI.ShowCardsBG();
            _recordCardsUI.ResetRecorder(); //重置记牌器
        }

    }
}
