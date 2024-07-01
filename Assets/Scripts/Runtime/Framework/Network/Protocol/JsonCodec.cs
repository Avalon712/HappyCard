using Newtonsoft.Json;
using System.Text;


namespace HappyCard
{
    public sealed class JsonCodec<T> : IDataDecoder, IDataEncoder
    {
        private readonly Encoding _encoding;

        public JsonCodec(Encoding encoding)
        {
            _encoding = encoding;
        }

        public T Data { get; private set; }


        public object Decode(byte[] data)
        {
            Data = JsonConvert.DeserializeObject<T>(_encoding.GetString(data));
            return Data;
        }


        public byte[] Encode(object data)
        {
            return _encoding.GetBytes(JsonConvert.SerializeObject(data));
        }

    }
}
