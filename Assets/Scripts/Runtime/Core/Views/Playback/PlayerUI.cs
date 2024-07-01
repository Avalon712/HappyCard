using System.Collections.Generic;

namespace HappyCard
{
    public sealed class PlayerUI
    {
        private ShowOutCardsUI _outCardsUI;        //当前回合出的牌
        private ShowCardsUI _cardsUI;           //当前玩家的手牌
        private BoutStateUI _stateUI;           //回合状态

        public PlayerUI(ShowOutCardsUI outCardsUI, ShowCardsUI cardsUI, BoutStateUI stateUI)
        {
            _outCardsUI = outCardsUI;
            _cardsUI = cardsUI;
            _stateUI = stateUI;
        }

        public void RemoveHandCards(List<PokerCard> removed)
        {
            _cardsUI.RemoveCards(removed);
        }

        public void AddHandCards(List<PokerCard> adds)
        {
            _cardsUI.AddCards(adds);
        }


        public void ShowBout(Bout bout)
        {
            _outCardsUI.ShowOutCards(bout.OutCards);
            _stateUI.SetState(bout.State);
        }

        public void Reset()
        {
            _stateUI.SetState(BoutState.Start);
            _outCardsUI.ShowOutCards(null);
        }

        public void SortHandCards()
        {
            _cardsUI.Sort();
        }
    }
}
