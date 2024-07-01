using System.Collections.Generic;
using UniVue.View.Views;
using UniVue;
using UniVue.Utils;

namespace HappyCard
{
    public sealed class ShowCardsUI
    {
        private string _viewName;
        private ObservableList<CardInfo> _cards;

        public ShowCardsUI(string viewName)
        {
            _viewName = viewName;
            _cards = new ObservableList<CardInfo>(20);
            Vue.Router.GetView<ClampListView>(_viewName).BindList(_cards);
        }


        public void RemoveCards(List<PokerCard> removed)
        {
            Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
            removed.ForEach(card =>
            {
                if (_cards.Contains(cardInfos[card]))
                    _cards.Remove(cardInfos[card]);
            });
        }


        public void AddCards(List<PokerCard> adds)
        {
            Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
            adds.ForEach(card =>
            {
                if (!_cards.Contains(cardInfos[card]))
                    _cards.Add(cardInfos[card]);
            });
        }

        public void Sort()
        {
            _cards.Sort((c1, c2) => c2.PokerCard - c1.PokerCard);
        }
    }
}
