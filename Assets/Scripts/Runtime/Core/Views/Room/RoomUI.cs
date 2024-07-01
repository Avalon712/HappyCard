using UniVue.Evt;
using UniVue.Evt.Attr;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Room))]
    public sealed class RoomUI : EventRegister
    {

        [EventCall(nameof(Chat))]
        private void Chat(string message)
        {

        }


        [EventCall(nameof(StartGame))]
        private void StartGame()
        {
            NetworkManager.Instance.Service.SendBroadcast(nameof(SyncEvents.StartGame), null);
            SceneController.LoadGameScene(GameDataContainer.Instance.Room.Gameplay);
        }


        [EventCall(nameof(DestroyRoom))]
        private void DestroyRoom()
        {
            NetworkManager.Instance.Service.SendBroadcast(nameof(SyncEvents.DestroyRoom), null);
            SceneController.LoadScene(GameScenes.Main);
        }


        [EventCall(nameof(QuitRoom))]
        private void QuitRoom()
        {
            NetworkManager.Instance.Service.SendBroadcast(nameof(SyncEvents.QuitRoom), null);
            SceneController.LoadScene(GameScenes.Main);
        }


        [EventCall(nameof(Invite))]
        private void Invite(string playerId)
        {

        }

    }
}
