using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.View.Views;

namespace HappyCard
{
    [EventCallAutowire(true,nameof(GameScenes.Login))]
    public sealed class SignupUI : EventRegister
    {
        [EventCall(nameof(Signup))]
        private void Signup(User user)
        {
            if (!GameDataService.HadRegistered())
            {
                GameDataService.RegisterNewUser(user);
                Vue.Router.Skip(nameof(GameUIs.SignupView), nameof(GameUIs.LoginView));
                LoginUI.AutoLogin();
            }
            else
                Vue.Router.GetView<TipView>(nameof(TipView)).Open("您已经注册过了哦，无需再进行注册！");
        }
    }
}
