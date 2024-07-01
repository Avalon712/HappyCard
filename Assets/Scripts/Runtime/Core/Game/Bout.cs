using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 回合数据
    /// </summary>
    public class Bout
    {
        /// <summary>
        /// 这个值由GameLoop负责填写
        /// </summary>
        public int PlayerID { get; set; } = int.MaxValue;

        /// <summary>
        /// 回合状态
        /// </summary>
        public BoutState State { get; set; }

        /// <summary>
        /// 当前回合玩家出的牌
        /// </summary>
        public List<PokerCard> OutCards { get; set; }

        /// <summary>
        /// 当前回合玩家出的牌的类型
        /// </summary>
        public PokerType PokerType { get; set; } = PokerType.None;
    }
}
