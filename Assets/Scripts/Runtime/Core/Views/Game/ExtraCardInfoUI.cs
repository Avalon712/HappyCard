using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Model;
using UniVue.View.Views;

namespace HappyCard
{
    public sealed class ExtraCardInfoUI
    {
        private AtomModel<List<Sprite>> _extraCards;
        private Sprite _cardBG;     //牌的背面

        public ExtraCardInfoUI()
        {
            Gameplay gameplay = GameDataContainer.Instance.Loop.File.Gameplay;
            _extraCards = AtomModelBuilder.Build("ExtraCardInfo", "Card", new List<Sprite>(gameplay == Gameplay.FightLandlord ? 3 : 1));

            //缓存一下牌的背面精灵图
            IView view = Vue.Router.GetView(nameof(GameUIs.ExtraCardInfoView));
            _cardBG = view.viewObject.transform.GetChild(0).GetComponent<Image>().sprite;
            ShowCardsBG();
            _extraCards.Bind(nameof(GameUIs.ExtraCardInfoView), false);
        }

        public void ShowCardsBG()
        {
            _extraCards.Value.Clear();
            int count = _extraCards.Value.Capacity;
            for (int i = 0; i < count; i++)
            {
                _extraCards.Value.Add(_cardBG);
            }
            (_extraCards as IBindableModel).NotifyAll();
        }


        public void ShowLandlordCards()
        {
            List<PokerCard> residualCards = GameDataContainer.Instance.Loop.File.OriginalCards[3];
            Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
            _extraCards.Value.Clear();
            residualCards.ForEach(card => _extraCards.Value.Add(cardInfos[card].Icon));
            (_extraCards as IBindableModel).NotifyAll();
        }
    }
}
