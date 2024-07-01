
namespace HappyCard
{
    public sealed class DestroyRoomHandler : ISyncHandler
    {
        public IDataDecoder Decoder => null;

        public IDataEncoder Encoder => null;

        public int SenderID { get; set; }
        public int ReceiveID { get; set; }


        public void OnHandle()
        {
            SceneController.LoadScene(GameScenes.Main);
        }
    }
}
