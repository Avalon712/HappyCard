using UniVue.Model;
using UniVue.ViewModel;

namespace HappyCard
{
    public sealed partial class GameSetting
    {
        [AutoNotify] private int _timer = 30;                //游戏中每个人最多思考时间
        [AutoNotify] private ShuffleMode _shuffleMode;       //洗牌模式
        [AutoNotify] private Gameplay _gameplay;             //游戏玩法
        [AutoNotify] private bool _allowUseRecorder = true;  //是否允许在游戏中使用记牌器
        [AutoNotify] private bool _allowUseProp = true;      //是否允许在游戏中使用道具

    }

    public enum ShuffleMode
    {
        [EnumAlias("随机洗牌")]
        Shuffle,
        [EnumAlias("不洗牌")]
        NoShuffle,
    }
}
