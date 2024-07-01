

namespace HappyCard
{
    public sealed class DislikeCmd : ICommand
    {
        public int PlayerID { get; set; }

        public DislikeCmd(int playerID)
        {
            PlayerID = playerID;
        }

        public void Execute()
        {
            
        }
    }
}
