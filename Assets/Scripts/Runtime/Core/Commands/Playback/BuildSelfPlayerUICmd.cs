

namespace HappyCard 
{
    public sealed class BuildSelfPlayerUICmd : ICommand
    {
        public PlayerUI PlayerUI { get; private set; }

        public void Execute()
        {
            ShowOutCardsUI outCardsUI = new ShowOutCardsUI(nameof(GameUIs.Playback_ShowOutCardsView));
            ShowCardsUI cardsUI = new ShowCardsUI(nameof(GameUIs.Playback_CardsView));
            BoutStateUI stateUI = new BoutStateUI(nameof(GameUIs.Playback_BoutStateView));
            PlayerUI = new PlayerUI(outCardsUI, cardsUI, stateUI);
        }

    }
}
