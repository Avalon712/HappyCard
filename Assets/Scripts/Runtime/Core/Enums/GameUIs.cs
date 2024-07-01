
namespace HappyCard
{
    public enum GameUIs
    {
        /// <summary>
        ///显示当前玩家回合状态信息的视图
        /// </summary>
        BoutStateView,

        /// <summary>
        /// 显示玩家手牌的视图
        /// </summary>
        CardsView,

        /// <summary>
        /// 显示玩家当前出的牌的视图
        /// </summary>
        OutCardsView,

        /// <summary>
        /// 斗地主控制面板
        /// </summary>
        FightLandlord_BoutPhaseView,

        /// <summary>
        /// 斗地主显示左边玩家的视图
        /// </summary>
        FightLandlord_LeftPlayerView,

        /// <summary>
        /// 斗地主显示右边玩家的视图
        /// </summary>
        FightLandlord_RightPlayerView,

        /// <summary>
        /// 显示当前计时器
        /// </summary>
        PhaseTimerView,

        /// <summary>
        /// 提示信息视图（瞬态）
        /// </summary>
        FastTipView,

        /// <summary>
        /// 记牌器视图
        /// </summary>
        RecordCardsView,

        /// <summary>
        /// 显示额外牌的信息，如：斗地主剩余的那三张牌、板子炮黑桃7叫的那张牌
        /// </summary>
        ExtraCardInfoView,

        /// <summary>
        /// 游戏结算UI
        /// </summary>
        GameOverView,

        /// <summary>
        /// 其它玩家自己的结算视图
        /// </summary>
        OthersResultView,

        /// <summary>
        /// 当前玩家自己的结算视图
        /// </summary>
        SelfResultView,

        /// <summary>
        /// 显示当前玩家对局中使用的炸弹的视图
        /// </summary>
        BombView,

        /// <summary>
        /// 显示当前对局中玩家使用的增益效果的道具的视图
        /// </summary>
        PropView,

        Playback_BoutStateView,
        Playback_CardsView,
        Playback_ControlPanelView,
        Playback_ShowOutCardsView,
        Playback_FightLandlord_RightPlayerView,
        Playback_FightLandlord_LeftPlayerView,

        /// <summary>
        /// Login场景的登录视图
        /// </summary>
        LoginView,

        /// <summary>
        /// Login场景的注册视图
        /// </summary>
        SignupView,

        /// <summary>
        /// Main场景的主视图
        /// </summary>
        MainView,

        /// <summary>
        /// Main场景的玩法视图
        /// </summary>
        GameplayView,

        /// <summary>
        /// Main场景的创建房间视图
        /// </summary>
        CreateRoomView,

        /// <summary>
        /// Room场景的主视图
        /// </summary>
        RoomView,
        
        /// <summary>
        /// Room场景中显示当前房间中的玩家的视图
        /// </summary>
        PlayersView,

        /// <summary>
        /// Game场景中显示当前玩家自己状态的视图
        /// </summary>
        SelfView,
    }
}
