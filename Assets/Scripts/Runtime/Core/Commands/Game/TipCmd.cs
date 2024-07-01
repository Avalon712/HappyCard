using System.Collections.Generic;

namespace HappyCard
{
    public sealed class TipCmd : ICommand
    {
        private int _playerID;

        public List<PokerCard> TipCards { get; private set; }

        public PokerType PokerType { get; private set; }

        public TipCmd(int playerID)
        {
            _playerID = playerID;
            TipCards = new List<PokerCard>();
        }

        public void Execute()
        {
            GameLoop loop = GameDataContainer.Instance.Loop;
            Bout lastOutCardBout = loop.LastOutCardBout;
            List<PokerCard> cards = loop.GetCards(_playerID);

            PokerType pokerType;
            if (lastOutCardBout == null || lastOutCardBout.PlayerID == _playerID)
                pokerType = loop.Rule.GetTipCards(cards, TipCards);
            else
                loop.Rule.GetTipCards(cards, lastOutCardBout.OutCards, lastOutCardBout.PokerType, TipCards, out pokerType);

            PokerType = pokerType;
        }
    }
}
