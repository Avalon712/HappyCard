namespace HappyCard
{
    /// <summary>
    /// 监听某个命令的执行，在命令执行前调用监听器的ListenTo()方法 ===> 这样可以为命令参数传递
    /// </summary>
    public interface ICmdListener
    {
        /// <summary>
        /// 命令执行前
        /// </summary>
        void BeforeCmdExecute(ICommand cmd) { }

        /// <summary>
        /// 命令执行后
        /// </summary>
        void AfterCmdExecute(ICommand cmd) { }

        /// <summary>
        /// 监听级别
        /// </summary>
        ListenLevel Level { get => ListenLevel.Permanent; }
    }

    public enum ListenLevel
    {
        /// <summary>
        /// 只监听一次
        /// </summary>
        One,
        /// <summary>
        /// 永久监听
        /// </summary>
        Permanent,
    }
}
