namespace HappyCard
{
    public sealed class PrepareCmd : ICommand
    {
        public void Execute()
        {
            RoomInfo room = GameDataContainer.Instance.Room;

            //单机模式下点击准备则直接开始游戏
            if (room.Status == HayypCard.NetworkStatus.Local)
                GameDataContainer.Instance.Loop.StartLoop();
            
            //其它模式则需要发送同步事件

        }
    }
}
