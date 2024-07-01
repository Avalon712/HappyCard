

using System;

namespace HappyCard
{
    public sealed class WANProtocolCodec : IProtocolCodec
    {

        public byte[] Encode(ref TransferProtocol protocol)
        {
            throw new NotImplementedException();
        }

        public TransferProtocol Decode(ReadOnlySpan<byte> protocolData)
        {
            throw new NotImplementedException();
        }
    }
}
