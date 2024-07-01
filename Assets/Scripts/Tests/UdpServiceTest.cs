using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HappyCard
{
    public class UdpServiceTest : MonoBehaviour
    {
        public int _senderID = 0;
        public TMP_Text _displayReceiveContent;
        public TMP_Text _endPoint;
        public TMP_InputField _receiverIDInput;
        public TMP_InputField _contentInput;
        public Button _sendBtn;
        public Button _broadcastBtn;

        private UdpSyncService _service;


        private void Awake()
        {
            Encoding encoding = NetworkManager.Instance.EncodeProtocol;
            _service = new UdpSyncService(_senderID, encoding);
            //NetworkManager.Instance.EncodProtocol = Encoding.Unicode;
            NetworkManager.Instance.AddHandler("Test", new UdpHandler(_displayReceiveContent, encoding));
            NetworkManager.Instance.Service = _service;
            _endPoint.text = _service.HostEP.ToString();
            BindBtnEvts();
        }

        private void BindBtnEvts()
        {
            _sendBtn.onClick.AddListener(SendTo);
            _broadcastBtn.onClick.AddListener(SendBroadcast);
        }

        private void SendTo()
        {
            _service.SendTo(int.Parse(_receiverIDInput.text), "Test", _contentInput.text);
        }

        private void SendBroadcast()
        {
            _service.SendBroadcast("Test", _contentInput.text);
        }

        private void OnDestroy()
        {
            _service.Dispose();
        }
    }

    public sealed class UdpHandler : ISyncHandler
    {
        private TMP_Text _text;
        private JsonCodec<Message> _codec;

        public UdpHandler(TMP_Text text,Encoding encoding)
        {
            _text = text;
            _codec = new JsonCodec<Message>(encoding);
        }

        public IDataDecoder Decoder => _codec;

        public IDataEncoder Encoder => _codec;

        public int SenderID { get ; set; }

        public int ReceiveID { get; set; }


        public void OnHandle()
        {
            _text.text = $"ReceiverID: {ReceiveID}\n SenderID: {SenderID}\n Content: {_codec.Data.Info}";
        }
    }

    public struct Message
    {
        public string Info { get; set; }
    }
}
