using HayypCard;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HappyCard
{
    public sealed class NetworkManager : UnitySingleton<NetworkManager>
    {
        private float _time;
        private Dictionary<string, ISyncHandler> _handlers = new();
        private ConcurrentQueue<string> _datagrams = new();

        public int SyncFrequency { get; set; } = 2; //不需要太高的同步频率，一秒同步两次就差不多了

        public Encoding EncodeProtocol { get; set; } = Encoding.UTF8;

        public ISyncService Service { get; set; }

        public NetworkStatus Status { get; set; } = NetworkStatus.LAN;


        public ISyncHandler GetHandler(string eventName)
        {
            _handlers.TryGetValue(eventName, out ISyncHandler handler);
            return handler;
        }

        public T GetHandler<T>(string eventName) where T : ISyncHandler
        {
            _handlers.TryGetValue(eventName, out ISyncHandler handler);
            return (T)handler;
        }

        public NetworkManager AddHandler(string eventName,ISyncHandler handler)
        {
            _handlers.TryAdd(eventName, handler);
            return this;
        }


        public void RemoveHandler(string eventName)
        {
            _handlers.Remove(eventName);
        }


        public void ClearHandlers()
        {
            _handlers.Clear();
        }

        public void AddDatagram(string eventName)
        {
            _datagrams.Enqueue(eventName);
        }


        private void FixedUpdate()
        {
            _time += Time.fixedDeltaTime;
            if(_time >= 1f / SyncFrequency)
            {
                _time = 0;
                if(_datagrams.TryDequeue(out string eventName))
                {
                    GetHandler(eventName).OnHandle();
                }
            }
        }
    }
}
