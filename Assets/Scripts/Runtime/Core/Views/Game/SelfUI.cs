namespace HappyCard
{
    public class SelfUI : IBoutListener, ICmdListener
    {
        private ShowOutCardsUI _outCardsUI;
        private BoutStateUI _stateUI;

        public SelfUI()
        {
            _outCardsUI = new ShowOutCardsUI(nameof(GameUIs.OutCardsView));
            _stateUI = new BoutStateUI(nameof(GameUIs.BoutStateView));

            CmdExecutor.Instance.AddListener(this);
        }


        public void OnBout(BoutState lastState)
        {
            _outCardsUI.Clear();
            _stateUI.SetState(BoutState.Start);//隐藏上回合的状态
        }

        public void OnEndBout(Bout bout)
        {
            //string outCards = bout.OutCards == null ? " " : string.Join(", ", bout.OutCards);
            //Debug.Log($"Self: PlayerID={bout.PlayerID}  BoutState={bout.State} PokerType={bout.PokerType}  OutCards=[{outCards}]");

            _outCardsUI.ShowOutCards(bout.OutCards);
            _stateUI.SetState(bout.State);
        }

        public void BeforeCmdExecute(ICommand cmd)
        {
            if (cmd is PrepareCmd)
                _stateUI.SetState(BoutState.Prepared);
        }

        public void OnPlayAllCards()
        {
            _stateUI.SetState(BoutState.End);
        }

        public void OnGameOver(GameFile file)
        {
            _outCardsUI.Clear();
            _stateUI.SetState(BoutState.OnPrepare);
        }
    }
}
