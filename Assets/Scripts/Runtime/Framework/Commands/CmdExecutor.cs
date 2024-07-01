using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 命名执行器，负责所有命令的执行和调度
    /// </summary>
    public sealed class CmdExecutor : UnitySingleton<CmdExecutor>
    {
        private List<ICmdListener> _listeners = new List<ICmdListener>();

        private Queue<ICommand> _cmds = new Queue<ICommand>();

        public CmdExecutor AddCmd(ICommand cmd)
        {
            _cmds.Enqueue(cmd);
            return this;
        }

        public CmdExecutor AddListener(ICmdListener listener)
        {
            _listeners.Add(listener);
            return this;
        }

        /// <summary>
        /// 立即执行命令
        /// </summary>
        public void ExecuteCmd(ICommand cmd)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                ICmdListener listener = _listeners[i];
                listener.BeforeCmdExecute(cmd);
            }

            cmd.Execute();

            for (int i = 0; i < _listeners.Count; i++)
            {
                ICmdListener listener = _listeners[i];
                listener.AfterCmdExecute(cmd);
                if (listener.Level == ListenLevel.One)
                    ListUtils.TailDelete(_listeners, i--);
            }
        }

        /// <summary>
        /// 场景切换时调用
        /// </summary>
        public void ClearListeners()
        {
            _listeners.Clear();
        }


        private void Awake()
        {
            _listeners = new List<ICmdListener>();
            _cmds = new Queue<ICommand>();
        }

        private void Update()
        {
            if (_cmds.Count > 0)
            {
                ExecuteCmd(_cmds.Dequeue());
            }
        }

        private void OnDestroy()
        {
            _cmds.Clear();
            _listeners.Clear();
            _cmds = null;
            _listeners = null;
        }
    }
}
