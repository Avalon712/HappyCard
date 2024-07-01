

namespace HappyCard
{
    public sealed class QuitRoomHandler : ISyncHandler
    {
        public IDataDecoder Decoder => null;

        public IDataEncoder Encoder => null;

        public int SenderID { get ; set ; }
        public int ReceiveID { get ; set ; }

        public void OnHandle()
        {
            //玩家SenderID离开房间
            GameDataContainer.Instance.Room.Players.RemoveAll(p => p.ID == SenderID);
        }
    }
}
