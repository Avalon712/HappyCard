namespace HappyCard
{
    public sealed class OnTimerEndCmd : ICommand
    {
        public void Execute()
        {
            //如果上家有人出牌择自动选择不要
            if (CanPass())
                CmdExecutor.Instance.AddCmd(new PassCmd());

            //如果上家没有人出牌择则进行提示后出牌
            else
            {
                CmdExecutor.Instance.AddCmd(new TipCmd(GameDataContainer.Instance.Self.ID));
                CmdExecutor.Instance.AddCmd(new OutCardCmd());
            }
        }

        private bool CanPass()
        {
            int currentPlayerID = GameDataContainer.Instance.Self.ID;
            int lastOutCardPlayerID = GameDataContainer.Instance.Loop.LastOutCardBout.PlayerID;
            return currentPlayerID != lastOutCardPlayerID;
        }
    }
}
