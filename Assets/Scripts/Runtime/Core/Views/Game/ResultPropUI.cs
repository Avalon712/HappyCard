using UniVue;
using UniVue.Utils;
using UniVue.View.Views;

namespace HappyCard
{
    /// <summary>
    /// SelfResultUI中显示玩家使用的道具信息的UI
    /// </summary>
    public sealed class ResultPropUI
    {
        private ObservableList<PropInfo> _propInfos;

        public ResultPropUI()
        {
            _propInfos = new ObservableList<PropInfo>();
            Vue.Router.GetView<ListView>(nameof(GameUIs.PropView)).BindList(_propInfos);
        }

        public void ShowProps(PropInfo propInfo)
        {
            _propInfos.Add(propInfo);
        }

        public void Clear()
        {
            _propInfos.Clear();
        }
    }
}
