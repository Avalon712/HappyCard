using Newtonsoft.Json;
using System;
using System.Text;

namespace HappyCard
{
    public sealed class LANProtocolCodec : IProtocolCodec
    {
        private Encoding _encoding;

        public LANProtocolCodec(Encoding encoding)
        {
            _encoding = encoding;
        }

        public byte[] Encode(ref TransferProtocol protocol)
        {
            return _encoding.GetBytes(JsonConvert.SerializeObject(protocol));
        }

        public TransferProtocol Decode(ReadOnlySpan<byte> protocolData)
        {
            return JsonConvert.DeserializeObject<TransferProtocol>(_encoding.GetString(protocolData));
        }
    }
}
