
using System;

namespace HappyCard
{
    public interface IProtocolCodec
    {
        byte[] Encode(ref TransferProtocol protocol);

        TransferProtocol Decode(ReadOnlySpan<byte> protocolData);
    }
}
