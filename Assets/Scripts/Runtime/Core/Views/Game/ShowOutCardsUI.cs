using System.Collections.Generic;
using UniVue;
using UniVue.View.Views;

namespace HappyCard
{
    /// <summary>
    /// 显示玩家出的牌的UI
    /// </summary>
    public sealed class ShowOutCardsUI
    {
        private string _viewName;

        public ShowOutCardsUI(string viewName)
        {
            _viewName = viewName;
            ClampListView outCardView = Vue.Router.GetView<ClampListView>(viewName);
            outCardView.BindList(new List<CardInfo>());
        }

        public void ShowOutCards(List<PokerCard> cards)
        {
            ClampListView outCardView = Vue.Router.GetView<ClampListView>(_viewName);
            if (cards != null)
            {
                outCardView.Clear();
                Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
                cards.ForEach(card => outCardView.AddData(cardInfos[card]));
            }
            else
                outCardView.Clear();
        }

        public void Clear()
        {
            Vue.Router.GetView<ClampListView>(_viewName).Clear();
        }
    }
}
