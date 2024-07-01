using UnityEngine;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Utils;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Main))]
    public sealed class GameplayUI : EventRegister
    {
        [EventCall(nameof(StartMatch))]
        private void StartMatch()
        {
            switch (NetworkManager.Instance.Status)
            {               
                //局域网或单机则直接开始游戏
                case HayypCard.NetworkStatus.Local:
                case HayypCard.NetworkStatus.LAN:
                    {
                        Gameplay gameplay = GameDataContainer.Instance.GameSetting.Gameplay;
                        GameDataContainer.Instance.Room = CreateLocalRoom(gameplay);
                        SceneController.LoadGameScene(gameplay);
                    }
                    break;

                //广域网则进入匹配模式
                case HayypCard.NetworkStatus.WAN:
                    break;
            }

        }

        private RoomInfo CreateLocalRoom(Gameplay gameplay)
        {
            Player self = GameDataContainer.Instance.Self;

            RoomInfo room =  new RoomInfo()
            {
                ID = "localhost",
                OwnerID = self.ID,
                Players = new ObservableList<Player>((int)gameplay + 3) { self },
                ShouldPeopleNum = (int)gameplay + 3,
                OwnerName = self.Name,
                CurrentPeopleNum = 1,
                Gameplay = gameplay,
                Status = HayypCard.NetworkStatus.Local
            };

            //创建虚拟的本地玩家
            for (int i = 1; i < room.Players.Capacity; i++)
            {
                Player player = GameDataService.GetDefaultPlayer("人机" + i);
                Debug.Log(player.ID);
                room.Players.Add(player);
            }
            
            return room;
        }
    }
}
