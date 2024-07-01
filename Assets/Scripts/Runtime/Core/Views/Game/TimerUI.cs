using UniVue;
using UniVue.Model;
using UniVue.Tween;
using UniVue.Tween.Tweens;

namespace HappyCard
{
    public sealed class TimerUI
    {
        private string _viewName;
        private AtomModel<int> _timer;      //定时器
        private TweenTask _timerTask;
        private int TIME;

        public TimerUI(string viewName)
        {
            _viewName = viewName;
            _timer = AtomModelBuilder.Build("Timer", "Timer", 30);
            _timer.Bind(viewName, false);
            _timerTask = TweenBehavior.Timer(Decrease).Interval(1f).ExecuteNum(int.MaxValue);
            _timerTask.Pause();
        }

        public void StartTimer(int timer)
        {
            Vue.Router.Open(_viewName);
            _timer.Value = timer;
            TIME = timer;
            _timerTask.Play();
        }

        public void StopTimer()
        {
            Vue.Router.Close(_viewName);
            _timerTask.Pause();
        }

        private void Decrease()
        {
            _timer.Value = TIME--;

            if (TIME < 0)
            {
                CmdExecutor.Instance.AddCmd(new OnTimerEndCmd());
            }
        }
    }
}
