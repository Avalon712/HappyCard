using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HappyCard
{
    public sealed class UdpSyncService : ISyncService
    {
        private int _senderID;
        private IProtocolCodec _codec;
        private Socket _socket;
        private byte[] _receiveBuffer;
        private Dictionary<int, IPEndPoint> _playerIPs;         //key=playerID
        private ConcurrentQueue<ValueTuple<int, string, object>> _sendQueue;

        public IPEndPoint HostEP { get; private set; }

        public UdpSyncService(int senderID, Encoding encoding)
        {
            _playerIPs = new Dictionary<int, IPEndPoint>();
            _senderID = senderID;
            _receiveBuffer = new byte[1024 * 2]; //2KB
            _sendQueue = new ConcurrentQueue<(int, string, object)>();
            _codec = new LANProtocolCodec(encoding);
            Initialize();
        }

        private void Initialize()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //获取本机的一个随机临时端口
            HostEP = new IPEndPoint(NetworkUtils.GetDhcpIPv4Address(), UnityEngine.Random.Range(49152, 65535));
            try
            {
                _socket.Bind(HostEP);

                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0); //接受到消息后会自动赋值
                _socket.BeginReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref remoteEP, OnReceiveSync, remoteEP);

                LogHelper.Info($"UDP服务启动成功,EndPoint[{HostEP}]");
            }
            catch (Exception e)
            {
                LogHelper.Warn($"UDP服务启动失败！异常原因:{e.Message}");
                _socket?.Dispose();
            }
        }

     
        private void OnReceiveSync(IAsyncResult result)
        {
            try
            {
                EndPoint remoteEP = (EndPoint)result.AsyncState;
                int len = _socket.EndReceiveFrom(result, ref remoteEP);

                ReadOnlySpan<byte> dataBytes = _receiveBuffer.AsSpan();
                TransferProtocol protocol = _codec.Decode(dataBytes.Slice(0, len));

                NetworkManager manager = NetworkManager.Instance;
                ISyncHandler handler = manager.GetHandler(protocol.EventName);
                if (handler != null)
                {
                    object data = handler.Decoder?.Decode(protocol.Data);
                    manager.AddDatagram(protocol.EventName);
                    handler.SenderID = protocol.SenderID;
                    handler.ReceiveID = protocol.ReceiverID;
                }

                if (!_playerIPs.ContainsKey(protocol.SenderID))
                    _playerIPs.Add(protocol.SenderID, new IPEndPoint((remoteEP as IPEndPoint).Address, (remoteEP as IPEndPoint).Port));
                
                _socket.BeginReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref remoteEP, OnReceiveSync, remoteEP);
            }
            catch (Exception e)
            {
                LogHelper.Warn($"UDP服务异常:{e.Message}");
            }
        }


        private void OnSendSync(IAsyncResult result)
        {
            if (!_sendQueue.IsEmpty && _sendQueue.TryDequeue(out ValueTuple<int, string, object> datagram))
            {
                if (datagram.Item1 == int.MinValue)
                    SendBroadcast(ref datagram);
                else
                    SendTo(ref datagram);
            }
        }


        public void SendBroadcast(string eventName, object data)
        {
            if (_sendQueue.IsEmpty)
            {
                ValueTuple<int, string, object> datagram = (int.MinValue, eventName, data);
                SendBroadcast(ref datagram);
            }
            else
            {
                _sendQueue.Enqueue((int.MinValue, eventName, data));
            }
        }


        public void SendTo(int receiverID, string eventName, object data)
        {
            if (_sendQueue.IsEmpty)
            {
                ValueTuple<int, string, object> datagram = (receiverID, eventName, data);
                SendTo(ref datagram);
            }
            else
            {
                _sendQueue.Enqueue((receiverID, eventName, data));
            }
        }

        public void AddHostEP(int playerID, IPEndPoint endPoint)
        {
            _playerIPs.TryAdd(playerID, endPoint);
        }

        private void SendTo(ref ValueTuple<int, string, object> data)
        {
            if (_playerIPs.TryGetValue(data.Item1, out IPEndPoint endPoint) &&
                GenerateProtocol(data.Item1, data.Item2, data.Item3, out TransferProtocol protocol))
            {
                byte[] transferData = _codec.Encode(ref protocol);
                _socket.BeginSendTo(transferData, 0, transferData.Length, SocketFlags.None, endPoint, OnSendSync, null);
            }
        }


        private void SendBroadcast(ref ValueTuple<int, string, object> data)
        {
            if (GenerateProtocol(-1, data.Item2, data.Item3, out TransferProtocol protocol))
            {
                byte[] transferData = _codec.Encode(ref protocol);

                foreach (var endPoint in _playerIPs.Values)
                    _socket.BeginSendTo(transferData, 0, transferData.Length, SocketFlags.None, endPoint, OnSendSync, null);
            }
        }


        private bool GenerateProtocol(int receiverID, string eventName, object data, out TransferProtocol protocol)
        {
            NetworkManager manager = NetworkManager.Instance;
            ISyncHandler handler = manager.GetHandler(eventName);

            if (handler != null)
            {
                byte[] dataBytes = handler.Encoder?.Encode(data);
                protocol = new TransferProtocol()
                {
                    SenderID = _senderID,
                    ReceiverID = receiverID,
                    Data = dataBytes,
                    EventName = eventName
                };
            }
            else
                protocol = default;

            return protocol.EventName != null;
        }


        public void Dispose()
        {
            _socket.Dispose();
            _playerIPs.Clear();
            _sendQueue.Clear();
            HostEP = null;
            _sendQueue = null;
            _receiveBuffer = null;
            _socket = null;
            _codec = null;
            _playerIPs = null;
        }
    }
}
