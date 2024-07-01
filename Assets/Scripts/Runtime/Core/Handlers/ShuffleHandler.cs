using System.Collections.Generic;

namespace HappyCard
{
    public sealed class ShuffleHandler : ISyncHandler
    {
        private JsonCodec<List<PokerCard>[]> _codec = new JsonCodec<List<PokerCard>[]>(NetworkManager.Instance.EncodeProtocol);

        public IDataDecoder Decoder => _codec;

        public IDataEncoder Encoder => _codec;

        public int SenderID { get ; set; }

        public int ReceiveID { get; set ; }

        public void OnHandle()
        {
            GameDataContainer.Instance.Loop.SetPlayersCards(_codec.Data);
        }
    }
}
