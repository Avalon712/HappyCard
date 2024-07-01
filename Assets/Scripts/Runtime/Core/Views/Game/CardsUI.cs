using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Evt.Evts;
using UniVue.Tween;
using UniVue.View.Views;
using UniVue.View.Widgets;

namespace HappyCard
{
    /// <summary>
    /// 玩家的手牌显示
    /// </summary>
    public sealed class CardsUI : EventRegister, IBoutListener, ICmdListener
    {
        private List<PokerCard> _cards;
        private List<PokerCard> _selectedCards;
        private List<Image> _selectedCardsImgs;

        /// <summary>
        /// 获取当前玩家选中要出的牌（拷贝后的数据）
        /// </summary>
        private List<PokerCard> SelectedCards => _selectedCards.Count == 0 ? null : _selectedCards.GetRange(0, _selectedCards.Count);


        public CardsUI()
        {
            ClampListView cardsView = Vue.Router.GetView<ClampListView>(nameof(GameUIs.CardsView));
            cardsView.BindList(new List<CardInfo>());
            _selectedCards = new List<PokerCard>();
            _selectedCardsImgs = new List<Image>();

            CmdExecutor.Instance.AddListener(this);
        }

        public void OnDealCards(List<PokerCard> cards)
        {
            cards.Sort((p1, p2) => p2 - p1); //排序
            _cards = cards;
            ClampListView cardsView = Vue.Router.GetView<ClampListView>(nameof(GameUIs.CardsView));
            Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
            cards.ForEach(card => cardsView.AddData(cardInfos[card]));
        }

        public void OnGameOver(GameFile file)
        {
            Vue.Router.GetView<ClampListView>(nameof(GameUIs.CardsView)).Clear();
        }

        public void BeforeCmdExecute(ICommand cmd)
        {
            if (cmd is ResetCmd || cmd is PassCmd)
            {
                Reset();
            }
            else if (cmd is OutCardCmd outCardCmd)
            {
                outCardCmd.OutCards = SelectedCards;
            }
            else if (cmd is SuccessOutCardCmd)
            {
                //从玩家手牌中移除已经打出的牌
                _cards.RemoveAll(card => _selectedCards.Contains(card));
                //更新视图
                ClampListView cardsView = Vue.Router.GetView<ClampListView>(nameof(GameUIs.CardsView));
                Dictionary<PokerCard, CardInfo> cardInfos = GameDataContainer.Instance.PokerCards;
                _selectedCards.ForEach(card => cardsView.RemoveData(cardInfos[card]));
                Reset(false);
            }
        }

        public void AfterCmdExecute(ICommand cmd)
        {
            if (cmd is TipCmd tipCmd)
            {
                if (tipCmd.PokerType != PokerType.None)
                {
                    Reset();
                    _selectedCards.AddRange(tipCmd.TipCards);
                    //动画表现
                    Transform content = Vue.Router.GetView<ClampListView>(nameof(GameUIs.CardsView)).GetWidget<ClampList>().Content;
                    for (int i = 0; i < content.childCount; i++)
                    {
                        GameObject item = content.GetChild(i).gameObject;
                        if (item.activeSelf)
                        {
                            TMP_Text pokerCardTxt = item.transform.GetChild(0).GetComponent<TMP_Text>();
                            Image pokerIconImg = item.transform.GetChild(1).GetComponent<Image>();
                            PokerCard pokerCard = Enum.Parse<PokerCard>(pokerCardTxt.text);
                            if (_selectedCards.Contains(pokerCard))
                            {
                                Tween(pokerIconImg, true);
                                _selectedCardsImgs.Add(pokerIconImg);
                            }
                        }
                    }
                }
                else
                    Vue.Router.GetView<TipView>(nameof(GameUIs.FastTipView)).Open("当前没有大于上家的牌!");

            }
        }

        /// <summary>
        /// 重置所有牌的状态
        /// </summary>
        private void Reset(bool tween = true)
        {
            if (_selectedCardsImgs.Count > 0)
            {
                for (int i = 0; i < _selectedCardsImgs.Count; i++)
                    Tween(_selectedCardsImgs[i], false, tween);
                _selectedCardsImgs.Clear();
                _selectedCards.Clear();
            }
        }

        /// <summary>
        /// 动画效果
        /// </summary>
        private void Tween(Image img, bool up, bool tween = true)
        {
            Vector3 p = img.transform.localPosition;
            p.y = up ? p.y + 40 : p.y - 40;
            if (tween)
                TweenBehavior.DoLocalMove(img.transform, 0.1f, p); //动画效果
            else
                img.transform.localPosition = p;
        }


        [EventCall(nameof(Selected))]
        private void Selected(PokerCard pokerCard, UIEvent trigger)
        {
            Image img = trigger.GetEventUI<Button>().transform.parent.GetComponent<Image>();

            if (_selectedCards.Contains(pokerCard))
            {
                _selectedCards.Remove(pokerCard);
                _selectedCardsImgs.Remove(img);
                Tween(img, false);
            }
            else
            {
                _selectedCardsImgs.Add(img);
                _selectedCards.Add(pokerCard);
                Tween(img, true);
            }
        }

    }
}
