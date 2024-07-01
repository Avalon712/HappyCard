using System.Collections.Generic;
using UniVue.Model;

namespace HappyCard
{
    /// <summary>
    /// 记牌器UI
    /// </summary>
    public sealed class RecordCardsUI 
    {
        private GroupModel _recorder;

        public RecordCardsUI()
        {
            //创建模型
            _recorder = new GroupModel("Recorder", 15);
            for (int i = 3; i <= 17; i++)
                _recorder.AddProperty(new IntProperty(_recorder, i.ToString(), i >= 16 ? 1 : 4));
            _recorder.Bind(nameof(GameUIs.RecordCardsView), false);
        }


        public void ShowResidualCards(List<PokerCard> pokerCards)
        {
            if (pokerCards == null) return;

            //TODO: 只有在当前玩家使用了记牌器的情况下才显示还剩余的牌的数量
            for (int i = 0; i < pokerCards.Count; i++)
            {
                string propertyName = ((int)pokerCards[i] / 4 + 3).ToString();
                int current = _recorder.GetPropertyValue<int>(propertyName);
                _recorder.SetPropertyValue(propertyName, --current);
            }
        }


        public void ResetRecorder()
        {
            for (int i = 3; i <= 17; i++)
                _recorder.SetPropertyValue(i.ToString(), i >= 16 ? 1 : 4);
        }
    }
}
