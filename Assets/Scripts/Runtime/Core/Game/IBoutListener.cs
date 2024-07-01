using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 显示回合
    /// </summary>
    public interface IBoutListener
    {
        /// <summary>
        /// 开始发牌
        /// </summary>
        public void OnDealCards(List<PokerCard> cards) { }

        /// <summary>
        /// 到当前玩家的回合时
        /// </summary>
        /// <param name="lastBout">上家回合时的状态</param>
        public void OnBout(BoutState lastBout) { }

        /// <summary>
        /// 当前玩家回合结束时
        /// </summary>
        /// <param name="current">当前玩家的回合信息</param>
        /// 
        public void OnEndBout(Bout current) { }

        /// <summary>
        /// 当玩家出完所有的手牌
        /// </summary>
        public void OnPlayAllCards() { }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public void OnGameOver(GameFile file) { }
    }
}
