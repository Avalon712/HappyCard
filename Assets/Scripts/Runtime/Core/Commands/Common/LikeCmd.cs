namespace HappyCard
{
    /// <summary>
    /// 点赞
    /// </summary>
    public sealed class LikeCmd : ICommand
    {
        public int PlayerID { get; set; }

        public LikeCmd(int playerID)
        {
            PlayerID = playerID;
        }

        public void Execute()
        {

        }
    }
}
