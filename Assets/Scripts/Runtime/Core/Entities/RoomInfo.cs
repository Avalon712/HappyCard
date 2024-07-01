using HayypCard;
using UniVue.Model;
using UniVue.Utils;

namespace HappyCard
{
    public sealed partial class RoomInfo
    {
        [AutoNotify] private string _ID;
        [AutoNotify] private int _ownerID = int.MinValue;
        [AutoNotify] private string _ownerName;
        [AutoNotify] private int _shouldPeopleNum;
        [AutoNotify] private int _currentPeopleNum;
        [AutoNotify] private Gameplay _gameplay;
        [AutoNotify] private NetworkStatus _status;

        /// <summary>
        /// 当前房间中的人数
        /// </summary>
        public ObservableList<Player> Players { get; set; }

       
        /// <summary>
        /// 判断当前玩家是否是房主
        /// </summary>
        public bool IsOwner()
        {
            return _ownerID == GameDataContainer.Instance.Self.ID;
        }
    }
}
