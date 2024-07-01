using System.Collections.Generic;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Utils;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Main))]
    public sealed class CreatRoomUI : EventRegister
    {
        [EventCall(nameof(CreateRoom))]
        private void CreateRoom()
        {
            //1. 判断当前玩家是否有房卡

            //2. 消费一张房卡

            //3. 创建房间
            GameDataContainer container = GameDataContainer.Instance;
            Player self = container.Self;
            Gameplay gameplay = container.GameSetting.Gameplay;

            container.Room = new RoomInfo()
            {
                ID = IPEndPointUtils.EndPointToString(NetworkManager.Instance.Service.HostEP)+"-"+self.ID.ToString("X"),
                OwnerID = self.ID,
                Players = new ObservableList<Player>((int)gameplay + 3) { self },
                ShouldPeopleNum = (int)gameplay + 3,
                OwnerName = self.Name,
                CurrentPeopleNum = 1,
                Gameplay = gameplay,
                Status = HayypCard.NetworkStatus.LAN
            };

            //4. 进入Room场景
            SceneController.LoadScene(GameScenes.Room);
        }
    }
}
