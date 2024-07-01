using UniVue;
using UniVue.View.Views;

namespace HappyCard
{
    public sealed class PassCmd : ICommand
    {
        public void Execute()
        {
            if (CanPass())
            {
                Bout bout = new Bout() { State = BoutState.Pass };
                GameDataContainer.Instance.Loop.Next(bout);
            }
            else
                Vue.Router.GetView<TipView>(nameof(GameUIs.FastTipView)).Open("当前是您开始出牌，不可以跳过哦!");
        }

        private bool CanPass()
        {
            int currentPlayerID = GameDataContainer.Instance.Self.ID;
            int lastOutCardPlayerID = GameDataContainer.Instance.Loop.LastOutCardBout.PlayerID;
            return currentPlayerID != lastOutCardPlayerID;
        }
    }
}
