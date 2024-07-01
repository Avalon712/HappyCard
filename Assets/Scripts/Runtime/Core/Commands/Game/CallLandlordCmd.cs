using System;
using System.Collections.Generic;
using UniVue;
using UniVue.View.Views;

namespace HappyCard
{
    public sealed class CallLandlordCmd : ICommand
    {
        public void Execute()
        {
            MakeTeammates();
            AddResidualCardsAndRefreshView();

            Bout bout = new Bout() { State = BoutState.JiaoDiZhu };
            GameDataContainer.Instance.Loop.Phase = 2;  //更新阶段
            GameDataContainer.Instance.Loop.Next(bout, false);
        }

        private void MakeTeammates()
        {
            GameLoop loop = GameDataContainer.Instance.Loop;
            GameFile file = loop.File;
            List<ValueTuple<Player, IVirtualPlayer>> players = loop.VirtualPlayers;
            int landlordID = players[loop.CurrentIndex].Item1.ID;

            IVirtualPlayer peasant1 = null;
            IVirtualPlayer peasant2 = null;
            int peasantID1 = -1;
            int peasantID2 = -1;

            //找到两个农民
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Item1.ID != landlordID)
                {
                    if (peasant1 == null)
                    {
                        peasant1 = players[i].Item2;
                        peasantID1 = players[i].Item1.ID;
                    }
                    else
                    {
                        peasant2 = players[i].Item2;
                        peasantID2 = players[i].Item1.ID;
                    }
                }
            }

            peasant1.Teammate = peasant2;
            peasant2.Teammate = peasant1;

            for (int i = 0; i < players.Count; i++)
            {
                int teammateID = -1;
                if (players[i].Item1.ID == peasantID1)
                    teammateID = peasantID1;
                else if (players[i].Item1.ID == peasantID2)
                    teammateID = peasantID2;

                file.Teammates.Add((players[i].Item1.ID, teammateID));
            }

        }

        private void AddResidualCardsAndRefreshView()
        {
            GameLoop loop = GameDataContainer.Instance.Loop;
            int selfID = GameDataContainer.Instance.Self.ID;
            int currentID = loop.VirtualPlayers[loop.CurrentIndex].Item1.ID;
            GameFile file = loop.File;

            int index = -1;
            List<PokerCard> residual = loop.File.OriginalCards[3];
            List<PokerCard> cards = loop.GetCards(currentID, ref index);
            cards.AddRange(residual);
            file.OriginalCards[index].AddRange(residual);

            //如果是当前玩家叫的地主还有刷新一下显示玩家手牌的视图
            if (selfID == currentID)
            {
                ClampListView cardsView = Vue.Router.GetView<ClampListView>(nameof(GameUIs.CardsView));
                Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
                residual.ForEach(card => cardsView.AddData(cardInfos[card]));
                //重新排一下序
                cardsView.Sort<CardInfo>((c1, c2) => c2 .PokerCard - c1.PokerCard);
            }
        }
    }
}
