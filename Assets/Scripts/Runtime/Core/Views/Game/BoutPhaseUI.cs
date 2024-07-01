using UnityEngine;
using UniVue;
using UniVue.Evt;
using UniVue.Utils;
using UniVue.View.Views;

namespace HappyCard
{
    /// <summary>
    /// 控制当前玩家的选项显示，即当前回合应该展示的选项
    /// </summary>
    public abstract class BoutPhaseUI : EventRegister, IBoutListener
    {
        private TimerUI _timerUI;

        /// <summary>
        /// 这个Transform下面显示每个阶段的选项
        /// <para>-------斗地主---------</para>
        /// <para>phase0: 准备</para>
        /// <para>phase1: 叫/不叫</para>
        /// <para>phase2: 不要/出牌/重选</para>
        /// <para>-------板子炮---------</para>
        /// <para>phase0: 准备</para>
        /// <para>phase1: 包/不包</para>
        /// <para>phase2: 反包/不反</para>
        /// <para>phase3: 黑桃7玩家叫牌选项</para>
        /// <para>phase4: 不要/出牌/重选</para>
        /// <para>-------炸金花---------</para>
        /// <para>phase0: 准备</para>
        /// <para>phase1: 弃牌/翻牌/下注/互看/梭哈</para>
        /// </summary>
        private Transform _phases;

        public BoutPhaseUI(string viewName)
        {
            IView view = Vue.Router.GetView(viewName);
            _timerUI = new TimerUI(nameof(GameUIs.PhaseTimerView));
            _phases = GameObjectFindUtil.DepthFind("Phases", view.viewObject).transform;
        }

        public void OnBout(BoutState lastBout)
        {
            //显示定时器
            _timerUI.StartTimer(GameDataContainer.Instance.GameSetting.Timer);

            //根据玩家的选项显示指定的选项
            OpenPhaseOptions(GameDataContainer.Instance.Loop.Phase);
        }


        public void OnEndBout(Bout bout)
        {
            //关闭选项视图
            _timerUI.StopTimer();
        }

        public virtual void OnGameOver(GameFile file)
        {
            _timerUI.StopTimer();
            OpenPhaseOptions(0); //第一阶段是准备阶段
        }


        /// <summary>
        /// 关闭当前打开的选项
        /// </summary>
        protected void ClosePhaseOptions(int phase)
        {
            _phases.GetChild(phase).gameObject.SetActive(false);
        }

        /// <summary>
        /// 打开指定阶段的选项
        /// </summary>
        protected void OpenPhaseOptions(int phase)
        {
            _phases.GetChild(phase).gameObject.SetActive(true);
        }

    }
}
