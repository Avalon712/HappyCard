using System.Collections.Generic;

namespace HappyCard
{
    public abstract class VirtualPlayer : IVirtualPlayer
    {
        private List<IBoutListener> _listeners;

        public bool Finished { get; set; }

        public IVirtualPlayer Teammate { get; set; }

        public VirtualPlayer()
        {
            _listeners = new List<IBoutListener>(1);
        }

        public virtual void OnDeal(List<PokerCard> cards)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                _listeners[i].OnDealCards(cards);
            }
        }

        public virtual void OnBout(Bout last)
        {
            for (int i = 0; i < _listeners.Count; i++)
                _listeners[i].OnBout(last.State);
        }


        public virtual void OnEndBout(Gameplay gameplay, Bout current)
        {
            for (int i = 0; i < _listeners.Count; i++)
                _listeners[i].OnEndBout(current);
        }


        public virtual void OnGameOver(GameFile file)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                _listeners[i].OnGameOver(file);
            }
        }


        public virtual void OnPlayAllCards()
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                _listeners[i].OnPlayAllCards();
            }
        }

        public IVirtualPlayer AddBoutListener(IBoutListener listener)
        {
            _listeners.Add(listener);
            return this;
        }
    }
}
