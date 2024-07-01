using System;
using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 游戏结算
    /// </summary>
    public static class GameSettlement
    {
        /// <summary>
        /// 计算每个玩家的炸弹收益
        /// </summary>
        /// <param name="gameplay">游戏玩法</param>
        /// <param name="players">玩家</param>
        /// <param name="bouts">回合数据</param>
        /// <returns>key=playerID value=(coinReward, diamondReward, 炸弹牌)</returns>
        public static Dictionary<int, List<ValueTuple<int, int, List<PokerCard>>>> SettlePlayersBombReward(Gameplay gameplay, List<Bout> bouts, int playerCount)
        {
            Dictionary<int, List<ValueTuple<int, int, List<PokerCard>>>> results = new Dictionary<int, List<(int, int, List<PokerCard>)>>(playerCount);

            for (int i = 0; i < bouts.Count; i++)
            {
                Bout bout = bouts[i];
                if (bout.State == BoutState.OutCard && (bout.PokerType == PokerType.Bomb || bout.PokerType == PokerType.KingBomb))
                {
                    ValueTuple<int, int> reward = SettleBombReward(gameplay, bout.OutCards);
                    if (results.TryGetValue(bout.PlayerID, out List<ValueTuple<int, int, List<PokerCard>>> rewards))
                        rewards.Add((reward.Item1, reward.Item2, bout.OutCards));
                    else
                        results.Add(bout.PlayerID, new List<(int, int, List<PokerCard>)>() { (reward.Item1, reward.Item2, bout.OutCards) });
                }
            }

            return results;
        }

        /// <summary>
        /// 计算一个炸弹的奖励
        /// </summary>
        /// <remarks>
        /// <para>斗地主计算公式</para>
        /// <para>coin = 炸弹对应的牌码 * 100</para>
        /// <para>diamond = 炸弹对应的牌码</para>
        /// </remarks>
        /// <param name="gameplay">游戏玩法</param>
        /// <param name="bomb">炸弹</param>
        /// <returns>(Item1: CoinReward Item2: DiamondReward)</returns>
        public static ValueTuple<int, int> SettleBombReward(Gameplay gameplay, List<PokerCard> bomb)
        {
            int coinReward = 0;
            int diamondReward = 0;

            switch (gameplay)
            {
                case Gameplay.FightLandlord:
                    {
                        int code = (int)bomb[0] / 4 + 3;
                        coinReward = code * 100;
                        diamondReward = code;
                    }
                    break;
                case Gameplay.BanZiPao:
                    break;
                case Gameplay.FriedGoldenFlower:
                    break;
            }

            return (coinReward, diamondReward);
        }

        /// <summary>
        /// 斗地主输赢结算
        /// </summary>
        /// <remarks>
        /// <para>计算公式</para>
        /// <para>地主：coin = 2 * 1000 * (shuffleMode+1) * (win ? 1 : -1)</para>
        /// <para>      diamond = 10 * (shuffleMode+1) * (win ? 1 : 0)</para>
        /// <para>农民：coin = 1000 * (shuffleMode+1) * (win ? 1 : -1)</para>
        /// </remarks>
        /// <param name="shuffleMode">洗牌模式</param>
        /// <param name="win">true:赢</param>
        /// <param name="landlord">true:地主  false:农民</param>
        /// <returns>(Item1: Coin Item2: Diamond)</returns>
        public static ValueTuple<int, int> SettleFightLandlord(ShuffleMode shuffleMode, bool win, bool landlord)
        {
            int coin = 0;
            int diamond = 0;
            if (landlord)
            {
                coin = 2 * 1000 * ((int)shuffleMode + 1) * (win ? 1 : -1);
                diamond = 10 * ((int)shuffleMode + 1) * (win ? 1 : 0);
            }
            else
            {
                coin = 1000 * ((int)shuffleMode + 1) * (win ? 1 : -1);
            }
            return (coin, diamond);
        }
    }
}
