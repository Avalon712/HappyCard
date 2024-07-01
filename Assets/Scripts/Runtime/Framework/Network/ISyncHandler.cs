
namespace HappyCard
{
    public interface ISyncHandler
    {
        /// <summary>
        /// 数据解码器
        /// </summary>
        /// <remarks>如果发送的事件没有数据，解码器可以为null</remarks>
        IDataDecoder Decoder { get; }

        /// <summary>
        /// 数据编码器
        /// </summary>
        /// <remarks>如果发送的事件没有数据，编码器可以为null</remarks>
        IDataEncoder Encoder { get; }

        int SenderID { get; set; }

        int ReceiveID { get; set; }

        void OnHandle();
    }
}
