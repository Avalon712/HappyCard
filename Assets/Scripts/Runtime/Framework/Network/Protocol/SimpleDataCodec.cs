using System;
using System.Text;

namespace HappyCard
{
    /// <summary>
    /// 针对一些简单的类型：string/int/float类型的编解码器
    /// </summary>
    public sealed class SimpleDataCodec<T> : IDataDecoder, IDataEncoder
    {
        private enum SimpleType
        {
            None,
            String,
            Int,
            Float
        }

        private SimpleType _simpleType;
        private Encoding _encoding;

        public SimpleDataCodec(Encoding encoding)
        {
            _encoding = encoding;
            _simpleType = GetSimpleType();
            CheckType();
        }

        public T Data { get; private set; }

        public object Decode(byte[] data)
        {
            Data = (T)DoDecode(data);
            return Data;
        }

        public byte[] Encode(object data)
        {
            return DoEncode(data);
        }

        private object DoDecode(byte[] data)
        {
            switch (_simpleType)
            {
                case SimpleType.String:
                    return _encoding.GetString(data);
                case SimpleType.Int:
                    return int.Parse(_encoding.GetString(data));
                case SimpleType.Float:
                    return float.Parse(_encoding.GetString(data));
            }
            return default;
        }

        private byte[] DoEncode(object data)
        {
            switch (_simpleType)
            {
                case SimpleType.String:
                    return _encoding.GetBytes((string)data);
                case SimpleType.Int:
                    return _encoding.GetBytes(data.ToString());
                case SimpleType.Float:
                    return _encoding.GetBytes(data.ToString());
            }
            return null;
        }

        private SimpleType GetSimpleType()
        {
            Type type = typeof(T);
            if (type == typeof(string))
                return SimpleType.String;
            else if (type == typeof(int))
                return SimpleType.Int;
            else if (type == typeof(float))
                return SimpleType.Float;
            return SimpleType.None;
        }

        private void CheckType()
        {
            if (_simpleType == SimpleType.None)
                throw new NotSupportedException($"不被支持的类型{typeof(T).Name}！SimpleCodec只支持对string、int、float进行编解码");
        }
    }
}
