using UniVue.Evt;
using UniVue.Evt.Attr;

namespace HappyCard
{
    [EventCallAutowire(true, nameof(GameScenes.Main))]
    public sealed class MainUI : EventRegister
    {

    }
}
