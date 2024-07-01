using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.View.Views;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Login))]
    public sealed class AppealUI : EventRegister
    {
        [EventCall(nameof(Appeal))]
        private void Appeal(string email)
        {
            Vue.Router.GetView<TipView>(nameof(TipView)).Open("此功能尚未完成");
        }
    }
}
