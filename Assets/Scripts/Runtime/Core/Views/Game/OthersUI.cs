using UniVue;
using UniVue.Model;
using UniVue.View.Views;

namespace HappyCard
{
    /// <summary>
    /// 显示其他玩家的出牌等回合状态
    /// </summary>
    public class OthersUI : IBoutListener
    {
        private int _playerID;                          //此UI所显示的玩家的ID
        private ShowOutCardsUI _cardUI;
        private TimerUI _timerUI;
        private BoutStateUI _stateUI;
        private AtomModel<string> _residualCardsNum;    //玩家剩余牌的数量


        public OthersUI(Player player, string viewName)
        {
            _playerID = player.ID;

            IView view = Vue.Router.GetView(viewName);
            using (var it = view.GetNestedViews().GetEnumerator())
            {
                while (it.MoveNext())
                {
                    if (it.Current is ClampListView)
                        _cardUI = new ShowOutCardsUI(it.Current.name);
                    else if (it.Current.name.EndsWith("TimerView"))
                        _timerUI = new TimerUI(it.Current.name);
                    else
                        _stateUI = new BoutStateUI(it.Current.name);
                }
            }
            view.BindModel(player);

            _residualCardsNum = AtomModelBuilder.Build("GamingInfo", "Residual", string.Empty);
            _residualCardsNum.Bind(viewName, false);
        }

        public void OnBout(BoutState lastBout)
        {
            _cardUI.Clear();
            _stateUI.SetState(BoutState.Start);
            _timerUI.StartTimer(GameDataContainer.Instance.GameSetting.Timer);
        }

        public void OnEndBout(Bout bout)
        {
            //string outCards = bout.OutCards == null ? " " : string.Join(", ", bout.OutCards);
            //Debug.Log($"Others: PlayerID={bout.PlayerID}  BoutState={bout.State}  PokerType={bout.PokerType}  OutCards=[{outCards}]");

            _timerUI.StopTimer();
            _cardUI.ShowOutCards(bout.OutCards);
            _stateUI.SetState(bout.State);
            ShowResidualCardsNum(bout.State);
        }

        public void OnPlayAllCards()
        {
            _stateUI.SetState(BoutState.End);
        }


        public void OnGameOver(GameFile file)
        {
            _cardUI.Clear();
            _stateUI.SetState(BoutState.OnPrepare);
            _residualCardsNum.Value = string.Empty;
        }


        private void ShowResidualCardsNum(BoutState state)
        {
            //TODO: 如果玩家在没有使用道具的情况下，则只有当玩家剩余3张牌时才允许显示
            if (state == BoutState.OutCard)
            {
                _residualCardsNum.Value = "剩余" + GameDataContainer.Instance.Loop.GetCards(_playerID).Count;
            }
        }
    }
}
