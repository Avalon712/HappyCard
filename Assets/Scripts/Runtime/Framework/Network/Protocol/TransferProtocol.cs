
namespace HappyCard
{
    public struct TransferProtocol
    {
        /// <summary>
        /// 发送者的playerID
        /// </summary>
        public int SenderID { get; set; }


        /// <summary>
        /// 接收者的playerID
        /// </summary>
        /// <remarks>如果为广播则无需填写此值</remarks>
        public int ReceiverID { get; set; }


        public string EventName { get; set; }

        /// <summary>
        /// 数据载荷
        /// </summary>
        public byte[] Data { get; set; }
    }

}
