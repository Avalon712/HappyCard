namespace HappyCard
{
    public sealed class SuccessOutCardCmd : ICommand
    {
        public Bout Bout { get; set; }

        public void Execute()
        {
            GameDataContainer.Instance.Loop.Next(Bout);
        }

    }
}
