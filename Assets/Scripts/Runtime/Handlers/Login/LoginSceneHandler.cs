using HappyCard.Entities;
using HappyCard.Entities.Configs;
using HappyCard.Enums;
using HappyCard.Managers;
using HappyCard.Network.Entities;
using HayypCard.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.View.Views;

namespace HappyCard.Handlers
{
    [EventCallAutowire(true, nameof(GameScene.Login))]
    public sealed class LoginSceneHandler : EventRegister
    {
        #region ��¼�¼�

        [EventCall(nameof(GameEvent.Login))]
        public void HandLoginEvt(User user, bool remember)
        {
            LogHelper.Info($"\"�û���¼-Login\"�¼�������name={user.name}, password={user.password}, rememberMe={remember}");

            HttpInfo<User, Player> httpInfo = new(GameEvent.Login);
            httpInfo.requestData = user;

            httpInfo.onSuccessed = () =>
            {
                Player player = httpInfo.responseData;

                user.id = player.ID;
                user.lastLoginDate = DateTime.Now;
                //������ҵ���Ϸ����
                player.days = user.lastLoginDate.Subtract(user.registerDate).Days;

                //ֻ�й�ѡ��ס�Һ��д��浵
                if (remember) { GameDataManager.Instance.User = user; }

                GameDataManager.Instance.Player = httpInfo.responseData;

                SceneManager.LoadScene(nameof(GameScene.Main), LoadSceneMode.Single);
            };

            httpInfo.onFailed = () =>
            {
                Vue.Router.GetView<TipView>(nameof(GameUI.TipView)).Open(httpInfo.exception);
            };

            NetworkManager.Instance.SendHttpRequest(httpInfo, GameEvent.Login);
        }
        #endregion

        //----------------------------------------------------------------------------------------------

        #region ע���¼�

        [EventCall(nameof(GameEvent.Signup))]
        public void HandSignupEvt(User user)
        {
            LogHelper.Info($"\"�û�ע��-Signup\"�¼�������email={user.email}, name={user.name}, password={user.password}");

            user.registerDate = DateTime.Now;
            user.lastLoginDate = DateTime.Now;

            HttpInfo<User, User> httpInfo = new(GameEvent.Signup);
            httpInfo.requestData = user;

            httpInfo.onSuccessed = () =>
            {
                GameDataManager.Instance.User = httpInfo.responseData;

                Vue.Router.GetView<TipView>(nameof(GameUI.TipView)).Open("ע��ɹ������Խ��е�¼!");
            };

            httpInfo.onFailed = () =>
            {
                Vue.Router.GetView<TipView>(nameof(GameUI.TipView)).Open($"�쳣: {httpInfo.exception}��������������Ϣ��{httpInfo.raw}");
            };

            NetworkManager.Instance.SendHttpRequest(httpInfo, GameEvent.Signup);
        }
        #endregion

        //----------------------------------------------------------------------------------------------

        #region �˺������¼�
        [EventCall(nameof(GameEvent.Appeal))]
        public void HandAppealEvt(string email)
        {
            LogHelper.Info($"�û��˺������¼�������email={email}");
        }
        #endregion

        //----------------------------------------------------------------------------------------------

        #region �Զ���¼�¼�
        public void AutoLogin()
        {
            User user = GameDataManager.Instance.User ;

            if (user != null && !string.IsNullOrEmpty(user.password) && !string.IsNullOrEmpty(user.name))
            {
                //Ϊ��¼������Զ���ֵ
                Dictionary<string, object> args = new Dictionary<string, object>()
                    {
                        { nameof(user.name),user.name },
                        { nameof(user.password),user.password }
                    };
                Vue.Event.SetEventArgs(nameof(GameEvent.Login), nameof(GameUI.LoginView), args);
            }
        }
        #endregion

        //---------------------------------------------------------------------------------------

    }
}

