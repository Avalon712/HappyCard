using UnityEngine;
using UniVue.Model;

namespace HappyCard
{
    public sealed partial class CardInfo
    {
        [AutoNotify] private PokerCard _pokerCard;
        [AutoNotify] private Sprite _icon;

        public CardInfo(PokerCard pokerCard, Sprite icon)
        {
            _pokerCard = pokerCard;
            _icon = icon;
        }
    }
}
