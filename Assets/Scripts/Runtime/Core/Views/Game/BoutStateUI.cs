using UniVue.Model;

namespace HappyCard
{
    /// <summary>
    /// 显示当前回合状态的UI
    /// </summary>
    public sealed class BoutStateUI
    {
        private AtomModel<BoutState> _boutState;   //显示当前回合状态

        public BoutStateUI(string viewName)
        {
            _boutState = AtomModelBuilder.Build("Bout", "State", BoutState.OnPrepare);
            _boutState.Bind(viewName, false);
        }

        public void SetState(BoutState state)
        {
            _boutState.Value = state;
        }

    }
}
