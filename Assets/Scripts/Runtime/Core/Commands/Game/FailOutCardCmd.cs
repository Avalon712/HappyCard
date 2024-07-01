using UniVue;
using UniVue.View.Views;

namespace HappyCard
{
    public sealed class FailOutCardCmd : ICommand
    {
        public void Execute()
        {
            Vue.Router.GetView<TipView>(nameof(GameUIs.FastTipView)).Open("当前您的出牌不符合规则哦");
        }

    }
}
