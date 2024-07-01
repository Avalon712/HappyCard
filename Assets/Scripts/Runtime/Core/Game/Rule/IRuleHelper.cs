using System.Collections.Generic;

namespace HappyCard
{
    public interface IRuleHelper
    {
        /// <summary>
        /// 洗牌
        /// </summary>
        void Shuffle(List<PokerCard>[] results);

        /// <summary>
        /// 不洗牌
        /// </summary>
        void NoShuffle(List<PokerCard>[] results, int controlFactor = 50);

        /// <summary>
        /// 判断当前玩家的出牌是否符合出牌规则
        /// </summary>
        /// <param name="current">当前玩家出的牌</param>
        /// <param name="currentType">当前玩家出的牌的类型</param>
        /// <param name="last">上一个玩家出的牌</param>
        /// <param name="lastType">上一个玩家出的牌的类型</param>
        /// <remarks>这种方式需要提前指定当前玩家出的牌的类型以及上家出的牌的类型</remarks>
        /// <returns>true:符合出牌规则；false:不符合出牌规则</returns>
        bool Check(List<PokerCard> current, PokerType currentType, List<PokerCard> last, PokerType lastType);

        /// <summary>
        /// 判断当前玩家的出牌是否符合出牌规则，无需先获取当前出牌的类型。
        /// </summary>
        /// <param name="current">当前玩家出的牌</param>
        /// <param name="last">上一个玩家出的牌</param>
        /// <param name="lastType">上一个玩家出的牌的类型</param>
        /// <param name="currentType">当前玩家出牌类型</param>
        /// <remarks>此方法可以在无需提前知道当前玩家出的牌的类型</remarks>
        /// <returns>true:符合出牌规则；false:不符合出牌规则</returns>
        bool FastCheck(List<PokerCard> current, List<PokerCard> last, PokerType lastType, out PokerType currentType);

        /// <summary>
        /// 获取一副牌的类型
        /// </summary>
        /// <param name="cards">要判断的牌的类型</param>
        /// <returns>牌的类型</returns>
        PokerType GetPokerType(List<PokerCard> cards);

        /// <summary>
        /// 该当前玩家出牌且上家没有人出牌时提示玩家出牌
        /// </summary>
        /// <remarks>注意：不会从玩家的手牌中移除提示的牌，这可能需要你自己完成这一步</remarks>
        /// <param name="cards">玩家的手牌</param>
        /// <param name="tipCards">存储提示要出的牌</param>
        /// <returns>出牌类型</returns>
        PokerType GetTipCards(List<PokerCard> cards, List<PokerCard> tipCards);

        /// <summary>
        /// 出牌提示：从当前玩家的手牌中获取到一副比上家出的牌要大的牌型
        /// </summary>
        /// <remarks>注意：不会从玩家的手牌中移除提示的牌，这可能需要你自己完成这一步</remarks>
        /// <param name="cards">当前玩家的手牌</param>
        /// <param name="outCards">上家出的牌</param>
        /// <param name="outCardsType">上家出的牌的类型</param>
        /// <param name="tipCards">提示玩家出的牌</param>
        /// <param name="tipType">提示出的牌的类型，如果没有则为None</param>
        /// <returns>当前提示出的牌，如果没有说明当前玩家的手牌不存在大于上家的牌</returns>
        void GetTipCards(List<PokerCard> cards, List<PokerCard> outCards, PokerType outCardsType, List<PokerCard> tipCards, out PokerType tipType);
    }
}
