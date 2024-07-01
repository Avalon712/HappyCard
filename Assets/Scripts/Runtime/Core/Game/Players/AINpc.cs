using System.Collections.Generic;
using UniVue.Tween;
using Random = UnityEngine.Random;

namespace HappyCard
{
    /// <summary>
    /// NPC
    /// </summary>
    public sealed class AINpc
    {
        private int _playerID;
        private float _thinkTime;

        public AINpc(int playerID, float thinkTime)
        {
            _playerID = playerID;
            _thinkTime = thinkTime;
        }

        public List<PokerCard> Cards { get; set; }


        public void OnBout(Bout last)
        {
            //单机模式下如果玩家自己不叫则由NPC开始
            if (last.State == BoutState.BuJiao)
            {
                TweenBehavior.Timer(() => CmdExecutor.Instance.AddCmd(new CallLandlordCmd()))
                    .ExecuteNum(1).Delay(Random.Range(1, _thinkTime));
            }
            else if (last.State == BoutState.OutCard || last.State == BoutState.Pass || last.PlayerID == _playerID)
            {
                OutCard(last);
            }
            else if (last.State == BoutState.Start)
            {
                TweenBehavior.Timer(() => CmdExecutor.Instance.AddCmd(new NoCallLandlordCmd()))
                    .ExecuteNum(1).Delay(Random.Range(1, _thinkTime));
            }
        }

        private void OutCard(Bout last)
        {
            GameLoop loop = GameDataContainer.Instance.Loop;
            Bout lastOutCardBout = loop.LastOutCardBout;
            IRuleHelper rule = loop.Rule;
            List<PokerCard> tipCards = new List<PokerCard>();

            PokerType pokerType;
            if (lastOutCardBout == null || lastOutCardBout.PlayerID == _playerID || last.PlayerID == _playerID)
                pokerType = rule.GetTipCards(Cards, tipCards);
            else
                rule.GetTipCards(Cards, lastOutCardBout.OutCards, lastOutCardBout.PokerType, tipCards, out pokerType);

            Bout bout;
            if (pokerType == PokerType.None)
                bout = new Bout() { State = BoutState.Pass };
            else
                bout = new Bout() { State = BoutState.OutCard, OutCards = tipCards, PokerType = pokerType };

            Cards.RemoveAll(card => tipCards.Contains(card));
            TweenBehavior.Timer(() => loop.Next(bout)).ExecuteNum(1).Delay(Random.Range(1, _thinkTime));
        }

    }
}
