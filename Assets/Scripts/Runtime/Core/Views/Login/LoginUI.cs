using System.Collections.Generic;
using System.Text;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.View.Views;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Login))]
    public sealed class LoginUI : EventRegister
    {
        public static void AutoLogin()
        {
            //1.从本地加载保存的账号密码信息
            User local = GameDataService.GetLocalUserData();

            //2.赋值到UI上
            if(local != null)
            {
                Dictionary<string, object> args = new Dictionary<string, object>()
                {
                    {nameof(local.name),local.name },
                    {nameof(local.password),local.password },
                    {nameof(local.remember),local.remember }
                };
                Vue.Event.SetEventArgs(nameof(Login), nameof(GameUIs.LoginView), args);
            }
        }


        [EventCall(nameof(Login))]
        private void Login(User user)
        {
            if (GameDataService.HadRegistered())
            {
                User local = GameDataService.GetLocalUserData();
                if(local.name == user.name && local.password == user.password)
                {
                    PostprocessLogin();
                }
                else
                    Vue.Router.GetView<TipView>(nameof(TipView)).Open("您输入的账号存在错误请重新进行输入！");
            }
            else
                Vue.Router.GetView<TipView>(nameof(TipView)).Open("当前尚未进行用户注册呢，请先进行注册吧！");
        }


        private void PostprocessLogin()
        {
            GameDataService.InitGameData();
            InitNetworkService();
            SceneController.LoadScene(GameScenes.Main);
        }


        private void InitNetworkService()
        {
            //1. 尝试联机远程服务器

            //2. 无法联上远程服务器则设置为局域网模式
            NetworkManager.Instance.Service = new UdpSyncService(GameDataContainer.Instance.Self.ID, NetworkManager.Instance.EncodeProtocol);

        }
    }


}
