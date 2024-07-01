
namespace HappyCard
{
    public sealed class BoutHandler : ISyncHandler
    {
        private JsonCodec<Bout> _codec = new JsonCodec<Bout>(NetworkManager.Instance.EncodeProtocol);

        public IDataDecoder Decoder => _codec;

        public IDataEncoder Encoder => _codec;

        public int SenderID { get; set; }
        public int ReceiveID { get; set; }

        public void OnHandle()
        {
            GameDataContainer.Instance.Loop.Next(_codec.Data);
        }
    }
}
