using UnityEngine;
using UnityEngine.UI;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Model;
using UniVue.Utils;
using UniVue.View.Views;

namespace HappyCard
{
    /// <summary>
    /// 游戏结算时显示其它玩家结果的UI
    /// </summary>
    public sealed class OthersResultUI : EventRegister
    {
        private ObservableList<OthersResultInfo> _othersResults;

        public OthersResultUI()
        {
            _othersResults = new ObservableList<OthersResultInfo>(GameDataContainer.Instance.Room.Players.Count - 1);
            Vue.Router.GetView<ListView>(nameof(GameUIs.OthersResultView)).BindList(_othersResults);
        }


        public void SetResult(Player player, int coinIncome, int diamondIncome, Sprite resultIcon)
        {
            OthersResultInfo model = _othersResults.Find(model => model.PlayerID == player.ID);
            if (model == null)
                _othersResults.Add(new OthersResultInfo(player, coinIncome, diamondIncome, resultIcon));
            else
                SetResult(model, player, coinIncome, diamondIncome, resultIcon);
        }


        private void SetResult(OthersResultInfo model, Player player, int coinIncome, int diamondIncome, Sprite resultIcon)
        {
            //只对可能变化的属性进行设置
            model.PlayerLevel = player.Level;
            model.PlayerLikes = player.Likes;
            model.CoinIncome = coinIncome;
            model.DiamondIncome = diamondIncome;
            model.ResultIcon = resultIcon;
        }

        /// <summary>
        /// 重置每个玩家的点赞状态
        /// </summary>
        public void ResetLikeState()
        {
            GameObject viewObject = Vue.Router.GetView<ListView>(nameof(GameUIs.OthersResultView)).viewObject;
            using(var it = ComponentFindUtil.GetComponents<Toggle>(viewObject).GetEnumerator())
            {
                while (it.MoveNext())
                {
                    it.Current.isOn = false;
                }
            }
        }


        [EventCall(nameof(Like))]
        private void Like(string id, bool like)
        {
            //判断当前游戏是否已经结束状态
            if (GameDataContainer.Instance.Loop.GameState != GameState.GameOver) return;

            int playerID = int.Parse(id);
            OthersResultInfo model = _othersResults.Find(model => model.PlayerID == playerID);
            if (model != null)
            {
                if (like)
                {
                    model.PlayerLikes += 1;
                    CmdExecutor.Instance.AddCmd(new LikeCmd(playerID));
                }
                else
                {
                    model.PlayerLikes -= 1;
                    CmdExecutor.Instance.AddCmd(new DislikeCmd(playerID));
                }
            }
        }
    }

    public sealed partial class OthersResultInfo
    {
        [AutoNotify] private int _coinIncome;
        [AutoNotify] private int _diamondIncome;
        [AutoNotify] private Sprite _resultIcon;
        [AutoNotify] private Sprite _playerHeadIcon;
        [AutoNotify] private string _playerName;
        [AutoNotify] private int _playerLevel;
        [AutoNotify] private int _playerID;
        [AutoNotify] private int _playerLikes;

        public OthersResultInfo(Player player, int coinIncome, int diamondIncome, Sprite resultIcon)
        {
            _playerHeadIcon = player.HeadIcon;
            _playerID = player.ID;
            _playerLevel = player.Level;
            _playerLikes = player.Likes;
            _playerName = player.Name;
            _coinIncome = coinIncome;
            _diamondIncome = diamondIncome;
            _resultIcon = resultIcon;
        }
    }
}
