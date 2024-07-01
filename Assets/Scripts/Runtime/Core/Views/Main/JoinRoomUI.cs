using System;
using System.Net;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.View.Views;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Main))]
    public sealed class JoinRoomUI : EventRegister
    {
        [EventCall(nameof(ApplyForJoinRoom))]
        private void ApplyForJoinRoom(string roomId)
        {
            try
            {
                //将房间id解析为玩家id和玩家的ip地址
                string[] infos = roomId.Split('-');
                IPEndPoint endPoint = IPEndPointUtils.StringToEndPoint(infos[0]);
                int playerID = Convert.ToInt32(infos[1], 16);
                if (endPoint != null)
                {
                    NetworkManager.Instance.Service.AddHostEP(playerID, endPoint);
                    NetworkManager.Instance.Service.SendTo(playerID, nameof(SyncEvents.ApplyForJoinRoom), GameDataService.CopyPlayer(GameDataContainer.Instance.Self));
                }
            }
            catch(Exception e)
            {
#if UNITY_EDITOR
                LogHelper.Error($"房间号解析错误：{e.Message}");
#endif
                Vue.Router.GetView<TipView>(nameof(GameUIs.FastTipView)).Open("错误的房间号格式！");
            }
        }
    }
}
