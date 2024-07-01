namespace HappyCard
{
    /// <summary>
    /// 同步事件
    /// </summary>
    public enum SyncEvents
    {
        QuitRoom,

        DestroyRoom,

        StartGame,

        /// <summary>
        /// 洗牌
        /// </summary>
        Shuffle,

        /// <summary>
        /// 开始游戏循环
        /// </summary>
        StartLoop,

        /// <summary>
        /// 设置第一个开始做选项的人
        /// </summary>
        SetFirstMakeChoice,
        ApplyForJoinRoom,
    }
}
