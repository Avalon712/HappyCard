using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace HappyCard
{
    /// <summary>
    /// 游戏循环
    /// </summary>
    public sealed class GameLoop
    {
        #region 私有字段
        private List<ILoopListener> _listeners;                             //流程监听器
        #endregion

        #region 属性
        public IRuleHelper Rule { get; private set; }

        /// <summary>
        /// 指示当前应该显示那个阶段的选项
        /// </summary>
        /// <remarks>默认从准备阶段的下一个阶段开始</remarks>
        public int Phase { get; set; } = 1;

        public List<ValueTuple<Player, IVirtualPlayer>> VirtualPlayers { get; private set; }

        /// <summary>
        /// 当前回合玩家的索引
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// 上个出牌的回合
        /// </summary>
        public Bout LastOutCardBout { get; private set; }

        /// <summary>
        /// 本局游戏所有相关的信息
        /// </summary>
        public GameFile File { get; private set; }

        /// <summary>
        /// 所有玩家的手牌
        /// </summary>
        /// <remarks>如果为房主这个值由本地生成，不是房主则由网络传输同步完成SetPlayersCards()</remarks>
        public List<PokerCard>[] Cards { get; private set; }

        public GameState GameState { get; set; }


        #endregion

        #region 构造函数
        public GameLoop(Gameplay gameplay, IRuleHelper helper, List<ValueTuple<Player, IVirtualPlayer>> players)
        {
            VirtualPlayers = players;
            Rule = helper;
            _listeners = new List<ILoopListener>(1);
            File = new GameFile()
            {
                Gameplay = gameplay,
                Teammates = new List<(int, int)>((int)gameplay + 3),
                Bouts = new List<Bout>(20),
                Winners = new List<Player>(gameplay == Gameplay.FriedGoldenFlower ? 1 : 2),
                Losers = new List<Player>((int)gameplay + 2),
                CardIndexs = new List<(int, int)>((int)gameplay + 2),
                Sequence = new List<int>((int)gameplay + 2)
            };
            InitCardsContainer(gameplay);
        }
        #endregion

        #region 服务方法
        /// <summary>
        /// 获取指定玩家的手牌
        /// </summary>
        public List<PokerCard> GetCards(int playerID)
        {
            for (int i = 0; i < VirtualPlayers.Count; i++)
            {
                if (VirtualPlayers[i].Item1.ID == playerID)
                    return Cards[i];
            }
            return null;
        }

        /// <summary>
        /// 获取指定玩家的手牌
        /// </summary>
        public List<PokerCard> GetCards(int playerID, ref int index)
        {
            for (int i = 0; i < VirtualPlayers.Count; i++)
            {
                if (VirtualPlayers[i].Item1.ID == playerID)
                {
                    index = i;
                    return Cards[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加流程监听器
        /// </summary>
        public GameLoop AddListener<T>(T listener) where T : ILoopListener
        {
            _listeners.Add(listener);
            return this;
        }

        #endregion

        /// <summary>
        /// 开始游戏循环(都准备完成后将开始游戏循环)
        /// </summary>
        /// <remarks>将会从发牌阶段开始直到本局游戏结束</remarks>
        public void StartLoop()
        {
            //尚未初始话位置关系
            if (File.Sequence.Count == 0)
                for (int i = 0; i < VirtualPlayers.Count; i++)
                    File.Sequence.Add(VirtualPlayers[i].Item1.ID);

            Phase = 1;
            Reset();

            //只允许房主进行洗牌
            if (GameDataContainer.Instance.Room.IsOwner())
            {
                CmdExecutor.Instance.ExecuteCmd(new ShuffleCmd());
                CmdExecutor.Instance.ExecuteCmd(new SetFirstMakeChoicePlayerCmd());
                DealCards();
                CmdExecutor.Instance.ExecuteCmd(new StartLoopCmd());
            }
            else
                DealCards();

            GameState = GameState.InGame;
        }


        /// <summary>
        /// 进入下一个玩家的回合
        /// </summary>
        /// <param name="curr">当前玩家回合的数据</param>
        /// <param name="enterNext">是否真的进入下一个玩家的回合，如果为false，则下一个回合任然将从当前玩家开始</param>
        public void Next(Bout curr, bool enterNext = true)
        {
            IVirtualPlayer currentPlayer = VirtualPlayers[CurrentIndex].Item2;
            curr.PlayerID = VirtualPlayers[CurrentIndex].Item1.ID;

            //1. 记录当前回合
            File.Bouts.Add(curr);

            //2. 回调监听器函数
            for (int i = 0; i < _listeners.Count; i++)
                _listeners[i].OnNext(curr);

            //3. 记录上个玩家出的牌的信息
            if (curr.State == BoutState.OutCard)
                LastOutCardBout = curr;

            //4. 回调当前玩家的退出回合的函数
            currentPlayer.OnEndBout(File.Gameplay, curr);

            //5. 检查当前玩家的手牌是否已经出完所有的手牌
            if (Cards[CurrentIndex].Count == 0)
            {
                currentPlayer.Finished = true;
                currentPlayer.OnPlayAllCards();
            }

            //6. 检查游戏是否结束
            if (CheckGameOver())
            {
                GameState = GameState.GameOver;
                LastOutCardBout = null;

                FindAllWinners(VirtualPlayers, File);
                FindAllLosers(VirtualPlayers, File);

                for (int i = 0; i < VirtualPlayers.Count; i++)
                    VirtualPlayers[i].Item2.OnGameOver(File);

                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnGameOver(File);

                return;
            }

            //7. 下一个玩家
            int next = enterNext ? GetNext(CurrentIndex) : CurrentIndex;
            VirtualPlayers[next].Item2.OnBout(curr);
            CurrentIndex = next;
        }


        #region 开始循环的执行流程
        /// <summary>
        /// 重置上一局游戏的状态
        /// </summary>
        private void Reset()
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].Clear();
                File.OriginalCards[i].Clear();
            }

            File.CardIndexs.Clear();
            File.Winners.Clear();
            File.Losers.Clear();
            File.Bouts.Clear();
            File.Teammates.Clear();

            for (int i = 0; i < VirtualPlayers.Count; i++)
            {
                VirtualPlayers[i].Item2.Finished = false;
                VirtualPlayers[i].Item2.Teammate = null;
            }
        }

        /// <summary>
        /// 发牌
        /// </summary>
        private void DealCards()
        {
            for (int i = 0; i < VirtualPlayers.Count; i++)
            {
                VirtualPlayers[i].Item2.OnDeal(Cards[i]);
                File.CardIndexs.Add((VirtualPlayers[i].Item1.ID, i));
            }

            for (int i = 0; i < _listeners.Count; i++)
                _listeners[i].OnDealCards(File.Gameplay, Cards);

            File.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            for (int i = 0; i < Cards.Length; i++)
                Cards[i].ForEach(card => File.OriginalCards[i].Add(card));
        }


        /// <summary>
        /// 设置第一个开始做选择的人
        /// </summary>
        /// <remarks>主要填写的是玩家的顺序索引而不是玩家ID</remarks>
        public void SetFirstMakeChoicePlayer(int playerIndex)
        {
            CurrentIndex = playerIndex;
        }

        /// <summary>
        /// 设置每个玩家的手牌
        /// </summary>
        public void SetPlayersCards(List<PokerCard>[] cards)
        {
            //采用拷贝的方式而不是赋值，因为有可能其它地方持有对原来的手牌的引用
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].Clear();
                Cards[i].AddRange(cards[i]);
            }
        }


        #endregion

        #region 内部实现功能方法
        /// <summary>
        /// 当前玩家的下一个玩家
        /// </summary>
        private int GetNext(int current)
        {
            int next = (current + 1) % VirtualPlayers.Count;
            while (VirtualPlayers[next].Item2.Finished)
                next = (next + 1) % VirtualPlayers.Count;
            return next;
        }

        private void InitCardsContainer(Gameplay gameplay)
        {
            switch (gameplay)
            {
                case Gameplay.FightLandlord:
                    Cards = new List<PokerCard>[4] { new List<PokerCard>(20), new List<PokerCard>(20), new List<PokerCard>(20), new List<PokerCard>(3) };
                    File.OriginalCards = new List<PokerCard>[4] { new List<PokerCard>(20), new List<PokerCard>(20), new List<PokerCard>(20), new List<PokerCard>(3) };
                    break;
                case Gameplay.BanZiPao:
                    Cards = new List<PokerCard>[4] { new List<PokerCard>(13), new List<PokerCard>(13), new List<PokerCard>(13), new List<PokerCard>(13) };
                    File.OriginalCards = new List<PokerCard>[4] { new List<PokerCard>(13), new List<PokerCard>(13), new List<PokerCard>(13), new List<PokerCard>(13) };
                    break;
                case Gameplay.FriedGoldenFlower:
                    Cards = new List<PokerCard>[VirtualPlayers.Count];
                    File.OriginalCards = new List<PokerCard>[VirtualPlayers.Count];
                    for (int i = 0; i < Cards.Length; i++)
                    {
                        Cards[i] = new List<PokerCard>(3);
                        File.OriginalCards[i] = new List<PokerCard>(3);
                    }
                    break;
            }
        }

        private bool CheckGameOver()
        {
            switch (File.Gameplay)
            {
                case Gameplay.FightLandlord:
                    return CheckGameOver_FightLandlord();
                case Gameplay.BanZiPao:
                    break;
                case Gameplay.FriedGoldenFlower:
                    break;
            }
            return false;
        }


        private bool CheckGameOver_FightLandlord()
        {
            for (int i = 0; i < VirtualPlayers.Count; i++)
            {
                if (VirtualPlayers[i].Item2.Finished)
                    return true;
            }
            return false;
        }


        private void FindAllWinners(List<ValueTuple<Player, IVirtualPlayer>> players, GameFile file)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Item2.Finished)
                {
                    file.Winners.Add(players[i].Item1);
                    using (var it = FindAllTeammates(players, players[i].Item2).GetEnumerator())
                    {
                        while (it.MoveNext())
                            file.Winners.Add(it.Current);
                    }
                    return;
                }
            }
        }

        private void FindAllLosers(List<ValueTuple<Player, IVirtualPlayer>> players, GameFile file)
        {
            if (file.Winners.Count > 0)
            {
                players.ForEach(p =>
                {
                    if (!file.Winners.Contains(p.Item1))
                        file.Losers.Add(p.Item1);
                });
            }
        }

        private IEnumerable<Player> FindAllTeammates(List<ValueTuple<Player, IVirtualPlayer>> players, IVirtualPlayer player)
        {
            if (player.Teammate != null)
            {
                IVirtualPlayer original = player;

                for (int j = 0; j < players.Count; j++)
                {
                    if (players[j].Item2 == player.Teammate)
                    {
                        yield return players[j].Item1;

                        player = player.Teammate;

                        //检查是否回到了原点或是否到了终点
                        if (player == original || player == null) break;
                    }
                }
            }
        }

        #endregion
    }

    public enum GameState
    {
        /// <summary>
        /// 待开始
        /// </summary>
        ToBeStarted,

        /// <summary>
        /// 游戏中
        /// </summary>
        InGame,

        /// <summary>
        /// 游戏结束
        /// </summary>
        GameOver
    }
}
