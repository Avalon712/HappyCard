using UniVue.Evt;
using UniVue.Evt.Attr;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Room))]
    public sealed class JoinRoomNotifyUI : EventRegister
    {

        [EventCall(nameof(AcceptApplyForJoinRoom))]
        private void AcceptApplyForJoinRoom(string playerId)
        {

        }



        [EventCall(nameof(RefuseApplyForJoinRoom))]
        private void RefuseApplyForJoinRoom(string playerId)
        {

        }

    }
}
