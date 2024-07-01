using UniVue.Evt;
using UniVue.Evt.Attr;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Main))]
    public sealed class InviteNotifyUI : EventRegister
    {

        [EventCall(nameof(AcceptInvite))]
        private void AcceptInvite()
        {

        }


        [EventCall(nameof(RefuseInvite))]
        private void RefuseInvite()
        {

        }
    }
}
