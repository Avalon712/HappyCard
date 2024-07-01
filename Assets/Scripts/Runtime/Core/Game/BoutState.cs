using UniVue.ViewModel;

namespace HappyCard
{
    /// <summary>
    /// 玩家游戏中的状态
    /// </summary>
    public enum BoutState
    {
        /// <summary>
        /// 回合开始状态
        /// </summary>
        [EnumAlias("")]
        Start,

        /// <summary>
        /// 未准备
        /// </summary>
        [EnumAlias("未准备")]
        OnPrepare,

        /// <summary>
        /// 已准备
        /// </summary>
        [EnumAlias("已准备")]
        Prepared,

        /// <summary>
        /// 叫地主
        /// </summary>
        [EnumAlias("叫地主")]
        JiaoDiZhu,

        /// <summary>
        /// 不叫地主
        /// </summary>
        [EnumAlias("不叫")]
        BuJiao,

        /// <summary>
        /// 不要
        /// </summary>
        [EnumAlias("不要")]
        Pass,

        /// <summary>
        /// 出牌
        /// </summary>
        [EnumAlias("")] //赋值未空可以自动隐藏显示
        OutCard,

        /// <summary>
        /// 回合结束（已完牌状态）
        /// </summary>
        [EnumAlias("已完牌")]
        End,
    }
}
