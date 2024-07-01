
using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 虚拟玩家
    /// </summary>
    public interface IVirtualPlayer
    {
        /// <summary>
        /// 当前是否已经出完手牌
        /// </summary>
        /// <remarks>这个值由GameLoop进行控制</remarks>
        bool Finished { get; set; }

        /// <summary>
        /// 当前玩家的队友
        /// </summary>
        /// <remarks>这个值由GameLoop进行控制</remarks>
        IVirtualPlayer Teammate { get; set; }

        /// <summary>
        /// 当开始发牌时调用
        /// </summary>
        void OnDeal(List<PokerCard> cards);

        /// <summary>
        /// 到当前玩家的回合
        /// </summary>
        /// <param name="last">上家回合的信息</param>
        void OnBout(Bout last);

        /// <summary>
        /// 当前玩家的回合结束时调用
        /// </summary>
        /// <param name="gameplay">当前的游戏玩法</param>
        /// <param name="current">当前的玩家的回合状态</param>
        void OnEndBout(Gameplay gameplay, Bout current);

        /// <summary>
        /// 当前玩家打完所有的牌
        /// </summary>
        void OnPlayAllCards();

        /// <summary>
        /// 当前游戏结束
        /// </summary>
        void OnGameOver(GameFile file);

        /// <summary>
        /// 添加回合监听器
        /// </summary>
        IVirtualPlayer AddBoutListener(IBoutListener listener);
    }
}
