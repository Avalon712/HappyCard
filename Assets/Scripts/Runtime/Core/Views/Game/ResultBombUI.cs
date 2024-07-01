using System.Collections.Generic;
using UnityEngine;
using UniVue;
using UniVue.Model;
using UniVue.Utils;
using UniVue.View.Views;

namespace HappyCard
{
    /// <summary>
    /// SelfResultUI中显示炸弹信息的UI
    /// </summary>
    public sealed class ResultBombUI
    {
        private ObservableList<GroupModel> _bombInfos;

        public ResultBombUI()
        {
            _bombInfos = new ObservableList<GroupModel>();
            Vue.Router.GetView<ListView>(nameof(GameUIs.BombView)).BindList(_bombInfos);
        }

        public void AddBombInfo(int diamondReward, int coinReward, List<Sprite> cardIcons)
        {
            _bombInfos.Add(CreateModel(diamondReward, coinReward, cardIcons));
            Vue.Router.GetView<ListView>(nameof(GameUIs.BombView)).Refresh(true);
        }

        public void Clear()
        {
            _bombInfos.Clear();
        }

        private GroupModel CreateModel(int diamondReward, int coinReward, List<Sprite> cardIcons)
        {
            GroupModel group = new GroupModel("BombInfo", 3);
            group.AddProperty(new IntProperty(group, "DiamondReward", diamondReward))
                 .AddProperty(new IntProperty(group, "CoinReward", coinReward))
                 .AddProperty(new ListSpriteProperty(group, "CardIcons", cardIcons));
            return group;
        }
    }
}
