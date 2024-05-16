using HappyCard.Entities;
using HappyCard.Enums;
using HappyCard.Managers;
using HayypCard.Enums;
using HayypCard.Utils;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.View.Views;

namespace HappyCard.Handlers
{
    [EventCallAutowire(true, nameof(GameScene.Main))]
    public class ShopHandler : EventRegister
    {

        [EventCall(nameof(GameEvent.BuyHP))]
        private void BuyHP(string id)
        {
            LogHelper.Info($"\"����������Ʒ-BuyHP\"�¼���������Ʒ���{id}");
            Shop(id, ProductType.HP);
        }

        //-------------------------------------------------------------------------------------------------------

        [EventCall(nameof(GameEvent.BuyCoin))]
        private void BuyCoin(string id)
        {
            LogHelper.Info($"\"��������Ʒ-BuyCoin\"�¼���������Ʒ���{id}");
            Shop(id, ProductType.Coin);
        }

        //-------------------------------------------------------------------------------------------------------

        [EventCall(nameof(GameEvent.BuyDiamond))]
        private void BuyDiamond(string id)
        {
            LogHelper.Info($"\"������ʯ��Ʒ-BuyDiamond\"�¼���������Ʒ���{id}");
            Shop(id, ProductType.Diamond);
        }

        //-------------------------------------------------------------------------------------------------------

        [EventCall(nameof(GameEvent.BuyProp))]
        private void BuyProp(string id)
        {
            LogHelper.Info($"\"���������Ʒ-BuyProp\"�¼���������Ʒ���{id}");
            Shop(id, ProductType.Prop);
        }

        //--------------------------------------------------------------------------------------------------------

        /// <summary>
        /// ����ָ�����͡�ָ����Ʒ
        /// </summary>
        /// <param name="id">��Ʒ���</param>
        /// <param name="productType">��Ʒ����</param>
        public void Shop(string id,ProductType productType)
        {
            //1. ��ȡ��Ʒ
            Product product = GameDataManager.Instance.GetProduct(id, productType);
            //2. ���Ѽ���
            if (Consume(product))
            {
                Vue.Router.GetView<TipView>(nameof(GameUI.TipView)).Open("����ɹ���");
            }
            else
            {
                Vue.Router.GetView<TipView>(nameof(GameUI.TipView)).Open("����ʧ�ܣ����Ĺ��������в����죡");
            }
        }

        /// <summary>
        /// ���Ѽ���
        /// </summary>
        /// <returns>�Ƿ���ɹ���true:�ɹ�</returns>
        private bool Consume(Product product)
        {
            Player player = GameDataManager.Instance.Player;

            int coins = player.Coin;
            int hps = player.HP;
            int diamonds = player.Diamond;

            //1. ��Ԥ�����ٸ���
            switch (product.CurrencyType)
            {
                case CurrencyType.Coin:
                    coins -= product.RealPrice;
                    if (coins < 0) { return false; } else { player.Coin = coins; }
                    break;
                case CurrencyType.Diamond:
                    diamonds -= product.RealPrice;
                    if (diamonds < 0) { return false; } else { player.Diamond = diamonds; }
                    break;
                case CurrencyType.HP:
                    hps -= product.RealPrice;
                    if (hps < 0) { return false; } else { player.HP = hps; }
                    break;
            }

            //2. ������ҵĻ����й�����
            switch (product.ProductType)
            {
                case ProductType.Coin:
                    player.Coin += (int)(product.SellNum * (1 + product.Bonus / 100f));
                    break;

                case ProductType.Diamond:
                    player.Diamond += (int)(product.SellNum * (1 + product.Bonus / 100f));
                    break;

                case ProductType.HP:
                    player.HP += (int)(product.SellNum * (1 + product.Bonus / 100f));
                    break;

                case ProductType.Prop:
                    var item = GameDataManager.Instance.AddBagItem(PropTypeHelper.CNStringToEnum(product.ProductName));
                    if (item != null)
                    {
                        //ˢ����ͼ
                        Vue.Router.GetView<GridView>(nameof(GameUI.BagView)).AddData(item);
                    }
                    break;
            }

            return true;
        }


    }
}
