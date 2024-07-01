using System;
using System.Collections.Generic;

namespace HappyCard
{
    public sealed class GameFile
    {
        public string Date { get; set; }

        public Gameplay Gameplay { get; set; }

        /// <summary>
        /// 玩家之间的位置关系
        /// </summary>
        public List<int> Sequence { get; set; }

        /// <summary>
        /// 本局游戏中的玩家
        /// </summary>
        /// <remarks>
        /// <para>Item1:playerID  Item2:当前玩家的队友的playerID</para>
        /// <para>如果Item2=-1表示当前玩家没有队友</para>
        /// </remarks>
        public List<ValueTuple<int, int>> Teammates { get; set; }

        /// <summary>
        /// 初始手牌
        /// </summary>
        /// <remarks>Players[i]对应的手牌为OriginalCards[i]</remarks>
        public List<PokerCard>[] OriginalCards { get; set; }

        /// <summary>
        /// 每个玩家的原始手牌
        /// </summary>
        /// <remarks>Item1=playerID Item2=OriginalCards中的索引</remarks>
        public List<ValueTuple<int,int>> CardIndexs { get; set; }

        /// <summary>
        /// 游戏回合数据
        /// </summary>
        public List<Bout> Bouts { get; set; }

        /// <summary>
        /// 赢家
        /// </summary>
        /// <remarks>Item1:playerID  Item2:playerName</remarks>
        public List<Player> Winners { get; set; }

        /// <summary>
        /// 输家
        /// </summary>
        /// <remarks>Item1:playerID  Item2:playerName</remarks>
        public List<Player> Losers { get; set; }

        public GameFile() { }

        /// <summary>
        /// 获取指定玩家的原始手牌
        /// </summary>
        public List<PokerCard> GetOriginalCards(int playerID)
        {
            return OriginalCards[CardIndexs.Find(r => r.Item1 == playerID).Item2];
        }

        /// <summary>
        /// 获取当前玩家的下一个玩家的ID
        /// </summary>
        public int GetNext(int playerID)
        {
            int index = Sequence.IndexOf(playerID);
            return index >= 0 ? Sequence[(index + 1) % Sequence.Count] : -1;
        }
    }
}
