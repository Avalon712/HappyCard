
namespace HappyCard
{
    public sealed class StartLoopCmd : ICommand
    {
        public void Execute()
        {
            GameLoop loop = GameDataContainer.Instance.Loop;
            loop.VirtualPlayers[loop.CurrentIndex].Item2.OnBout(new Bout() { State = BoutState.Start });

            //如果当前是房主则向其它玩家发送同步
            if (GameDataContainer.Instance.Room.IsOwner())
            {
                NetworkManager.Instance.Service.SendBroadcast(nameof(SyncEvents.StartLoop), null);
            }
        }
    }
}
