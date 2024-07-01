using UnityEngine;
using UniVue;
using UniVue.Model;

namespace HappyCard
{
    /// <summary>
    /// 游戏结算时自己显示自己的结果的UI
    /// </summary>
    public sealed class SelfResultUI
    {
        private SelfResultInfo _resultInfo;

        public SelfResultUI()
        {
            _resultInfo = new SelfResultInfo();

            Vue.Router.GetView(nameof(GameUIs.SelfResultView)).BindModel(_resultInfo, false);
        }

        public void SetResult(int coinIncome, int diamondIncome, Sprite resultIcon)
        {
            _resultInfo.CoinIncome = coinIncome;
            _resultInfo.DiamondIncom = diamondIncome;
            _resultInfo.ResultIcon = resultIcon;
        }
    }

    public sealed partial class SelfResultInfo
    {
        [AutoNotify] private int _coinIncome;
        [AutoNotify] private int _diamondIncom;
        [AutoNotify] private Sprite _resultIcon;
    }
}
