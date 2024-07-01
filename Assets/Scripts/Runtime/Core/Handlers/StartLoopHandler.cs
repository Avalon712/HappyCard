
namespace HappyCard
{
    public sealed class StartLoopHandler : ISyncHandler
    {
        public IDataDecoder Decoder => null;

        public IDataEncoder Encoder => null;

        public int SenderID { get; set; }
        public int ReceiveID { get; set; }

        public void OnHandle()
        {
            GameDataContainer.Instance.Loop.StartLoop();
        }
    }
}
