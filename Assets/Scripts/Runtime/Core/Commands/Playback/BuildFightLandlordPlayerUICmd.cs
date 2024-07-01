using UniVue.View.Views;
using UniVue;

namespace HappyCard
{
    public sealed class BuildFightLandlordPlayerUICmd : ICommand
    {
        public PlayerUI LeftPlayerUI { get; private set; }

        public PlayerUI RightPlayerUI { get; private set; }


        public void Execute()
        {
            BuildFightLandlordPlayerUIs();
        }


        private void BuildFightLandlordPlayerUIs()
        {
            LeftPlayerUI = BuildPlayerUI(nameof(GameUIs.Playback_FightLandlord_LeftPlayerView));
            RightPlayerUI = BuildPlayerUI(nameof(GameUIs.Playback_FightLandlord_RightPlayerView));
        }


        private PlayerUI BuildPlayerUI(string rootViewName)
        {
            ShowOutCardsUI outCardsUI = null;
            ShowCardsUI cardsUI = null;
            BoutStateUI stateUI = null;
            IView view = Vue.Router.GetView(rootViewName);
            using (var it = view.GetNestedViews().GetEnumerator())
            {
                while (it.MoveNext())
                {
                    string name = it.Current.name;
                    if (name.EndsWith("ShowOutCardsView"))
                        outCardsUI = new ShowOutCardsUI(name);
                    else if (name.EndsWith("CardsView"))
                        cardsUI = new ShowCardsUI(name);
                    else if (name.EndsWith("BoutStateView"))
                        stateUI = new BoutStateUI(name);
                }
            }
            return new PlayerUI(outCardsUI, cardsUI, stateUI);
        }

    }
}
