using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 本地玩家
    /// </summary>
    public sealed class LocalPlayer : VirtualPlayer
    {
        private AINpc _npc;

        /// <summary>
        /// 单机模式下的虚拟玩家
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        /// <param name="isNpc">是否是NPC</param>
        /// <param name="thinkTime">当是NPC玩家时，指定一个最长思考时间来表现正在思考的效果</param>
        public LocalPlayer(int playerID, bool isNpc = true, float thinkTime = 1f)
        {
            _npc = isNpc ? new AINpc(playerID, thinkTime) : null;
        }


        public override void OnDeal(List<PokerCard> cards)
        {
            if (_npc != null)
                _npc.Cards = cards;

            base.OnDeal(cards);
        }

        public override void OnBout(Bout last)
        {
            base.OnBout(last);
            _npc?.OnBout(last);
        }

    }
}
