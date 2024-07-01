using System.Collections.Generic;
using UniVue;
using UniVue.View.Views;

namespace HappyCard
{
    public sealed class OutCardCmd : ICommand
    {
        public List<PokerCard> OutCards { get; set; }

        public void Execute()
        {
            if (OutCards == null || OutCards.Count == 0)
            {
                Vue.Router.GetView<TipView>(nameof(GameUIs.FastTipView)).Open("当前还没有选中任何牌呢");
                return;
            }

            GameLoop loop = GameDataContainer.Instance.Loop;
            int currentPlayerID = GameDataContainer.Instance.Self.ID;
            Bout lastOutCardBout = loop.LastOutCardBout;

            PokerType pokerType;
            bool right = true;
            if (lastOutCardBout == null || currentPlayerID == lastOutCardBout.PlayerID || lastOutCardBout.PlayerID == -1)
                pokerType = loop.Rule.GetPokerType(OutCards);
            else
                right = loop.Rule.FastCheck(OutCards, lastOutCardBout.OutCards, lastOutCardBout.PokerType, out pokerType);

            if (pokerType != PokerType.None && right)
            {
                Bout bout = new Bout() { State = BoutState.OutCard, OutCards = OutCards, PokerType = pokerType };
                CmdExecutor.Instance.AddCmd(new SuccessOutCardCmd() { Bout = bout });
            }
            else
                CmdExecutor.Instance.AddCmd(new FailOutCardCmd());
        }

    }
}
