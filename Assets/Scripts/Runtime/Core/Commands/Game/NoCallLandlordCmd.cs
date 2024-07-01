namespace HappyCard
{
    public sealed class NoCallLandlordCmd : ICommand
    {
        public void Execute()
        {
            Bout bout = new Bout() { State = BoutState.BuJiao };
            GameDataContainer.Instance.Loop.Next(bout);
        }

    }
}
