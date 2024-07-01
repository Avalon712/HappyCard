
namespace HappyCard
{
    public sealed class SetFirstMakeChoicePlayerHandler : ISyncHandler
    {
        private SimpleDataCodec<int> _codec = new SimpleDataCodec<int>(NetworkManager.Instance.EncodeProtocol);

        public IDataDecoder Decoder => _codec;

        public IDataEncoder Encoder => _codec;

        public int SenderID { get; set ; }

        public int ReceiveID { get; set; }

        public void OnHandle()
        {
            GameDataContainer.Instance.Loop.SetFirstMakeChoicePlayer(_codec.Data);
        }
    }
}
