using System.Collections.Generic;


namespace HappyCard
{
    public interface ILoopListener
    {
        public void OnDealCards(Gameplay gameplay, List<PokerCard>[] cards) { }

        public void OnNext(Bout bout) { }

        public void OnGameOver(GameFile file) { }
    }
}
