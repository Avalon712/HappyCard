using System;
using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 斗地主结算命令
    /// </summary>
    public sealed class FightLandlordSettlementCmd : ICommand
    {
        private List<Player> _winners;
        private List<Player> _losers;
        private List<Bout> _bouts;
        private ShuffleMode _shuffleMode;

        /// <summary>
        /// 所有玩家的炸弹收益
        /// </summary>
        /// <remarks>key = playerID value = List(coinReward, diamondReward, 炸弹牌)</remarks>
        public Dictionary<int, List<ValueTuple<int, int, List<PokerCard>>>> BombRewards { get; private set; }

        public List<Player> Winners => _winners;

        public List<Player> Losers => _losers;

        /// <summary>
        /// 所有玩家的总收益
        /// </summary>
        /// <remarks>key=playerID value=(coin, diamond)</remarks>
        public Dictionary<int, ValueTuple<int, int>> Incomes { get; private set; }

        public FightLandlordSettlementCmd(ShuffleMode shuffleMode, List<Player> winners, List<Player> losers, List<Bout> bouts)
        {
            _shuffleMode = shuffleMode;
            _winners = winners;
            _losers = losers;
            _bouts = bouts;
            BombRewards = null;
            Incomes = null;
        }

        public void Execute()
        {
            int playerCount = _winners.Count + _losers.Count;
            Incomes = new Dictionary<int, (int, int)>(playerCount);

            //1. 计算每个玩家的炸弹收益
            BombRewards = GameSettlement.SettlePlayersBombReward(Gameplay.FightLandlord, _bouts, playerCount);

            //2. 计算输赢收益
            Dictionary<int, ValueTuple<int, int>> incomes = new Dictionary<int, (int, int)>(playerCount);

            for (int i = 0; i < _winners.Count; i++)
                incomes.TryAdd(_winners[i].ID, GameSettlement.SettleFightLandlord(_shuffleMode, true, true));

            for (int i = 0; i < _losers.Count; i++)
                incomes.TryAdd(_losers[i].ID, GameSettlement.SettleFightLandlord(_shuffleMode, false, false));

            //3. 计算总收益
            foreach (var playerID in incomes.Keys)
            {
                //3.1 统计玩家炸弹的总收益
                int sumCoins = 0, sumDiamonds = 0;
                if (BombRewards.TryGetValue(playerID, out List<ValueTuple<int, int, List<PokerCard>>> bombs))
                {
                    for (int i = 0; i < bombs.Count; i++)
                    {
                        sumCoins += bombs[i].Item1;
                        sumDiamonds += bombs[i].Item2;
                    }
                }

                //3.2 计算总收益
                Incomes.Add(playerID, (incomes[playerID].Item1 + sumCoins, incomes[playerID].Item2 + sumDiamonds));
            }
        }

        public bool IsWinner(int playerID)
        {
            for (int i = 0; i < _winners.Count; i++)
            {
                if (_winners[i].ID == playerID)
                    return true;
            }
            return false;
        }

        public Player GetPlayer(int playerID)
        {
            for (int i = 0; i < _winners.Count; i++)
            {
                if (_winners[i].ID == playerID)
                    return _winners[i];
            }

            for (int i = 0; i < _losers.Count; i++)
            {
                if (_losers[i].ID == playerID)
                    return _losers[i];
            }

            return null;
        }
    }
}
