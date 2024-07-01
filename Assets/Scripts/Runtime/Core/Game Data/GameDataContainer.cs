using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// 全局共享数据访问
    /// </summary>
    public sealed class GameDataContainer
    {
        private readonly static GameDataContainer _container = new GameDataContainer();

        public static GameDataContainer Instance => _container;

        private GameDataContainer() { }

        /// <summary>
        /// 当前玩家自己
        /// </summary>
        public Player Self { get; set; }

        /// <summary>
        /// 游戏设置
        /// </summary>
        public GameSetting GameSetting { get; set; }

        /// <summary>
        /// 游戏循环控制
        /// </summary>
        public GameLoop Loop { get; set; }

        /// <summary>
        /// 所有的扑克牌信息
        /// </summary>
        public Dictionary<PokerCard, CardInfo> PokerCards { get; set; }

        /// <summary>
        /// 所有的道具信息
        /// </summary>
        public List<PropInfo> PropInfos { get; set; }

        /// <summary>
        /// 当前房间信息
        /// </summary>
        public RoomInfo Room { get; set; }
    }
}
