using UniVue.Evt.Attr;

namespace HappyCard
{
    /// <summary>
    /// 斗地主阶段选项事件处理
    /// </summary>
    public sealed class FightLandlord_BoutPhaseUI : BoutPhaseUI, ICmdListener
    {
        public FightLandlord_BoutPhaseUI(string viewName) : base(viewName) { CmdExecutor.Instance.AddListener(this); }


        public void BeforeCmdExecute(ICommand cmd)
        {
            if (cmd is SuccessOutCardCmd || cmd is PassCmd)
                ClosePhaseOptions(2);
        }


        public override void OnGameOver(GameFile file)
        {
            base.OnGameOver(file);
            ClosePhaseOptions(2);
        }


        [EventCall(nameof(Prepare))]
        private void Prepare()
        {
            CmdExecutor.Instance.AddCmd(new PrepareCmd());
            ClosePhaseOptions(0);
        }

        /// <summary>
        /// 叫地主
        /// </summary>
        [EventCall(nameof(CallLandlord))]
        private void CallLandlord()
        {
            CmdExecutor.Instance.AddCmd(new CallLandlordCmd());
            ClosePhaseOptions(1);
        }

        /// <summary>
        /// 不叫地主
        /// </summary>
        [EventCall(nameof(NoCallLandlord))]
        private void NoCallLandlord()
        {
            CmdExecutor.Instance.AddCmd(new NoCallLandlordCmd());
            ClosePhaseOptions(1);
        }


        /// <summary>
        /// 重置选牌
        /// </summary>
        [EventCall(nameof(Reset))]
        private void Reset()
        {
            CmdExecutor.Instance.AddCmd(new ResetCmd());
        }

        /// <summary>
        /// "不要"
        /// </summary>
        [EventCall(nameof(Pass))]
        private void Pass()
        {
            CmdExecutor.Instance.AddCmd(new PassCmd());
        }

        /// <summary>
        /// 获取提示
        /// </summary>
        [EventCall(nameof(Tip))]
        private void Tip()
        {
            CmdExecutor.Instance.AddCmd(new TipCmd(GameDataContainer.Instance.Self.ID));
        }

        /// <summary>
        /// 出牌
        /// </summary>
        [EventCall(nameof(OutCard))]
        private void OutCard()
        {
            CmdExecutor.Instance.AddCmd(new OutCardCmd());
        }

    }

}
