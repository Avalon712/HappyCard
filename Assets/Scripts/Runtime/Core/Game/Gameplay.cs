
using UniVue.ViewModel;

namespace HappyCard
{
    /// <summary>
    /// 游戏玩法
    /// </summary>
    public enum Gameplay
    {
        /// <summary>
        /// 斗地主
        /// </summary>
        [EnumAlias("斗地主")]
        FightLandlord,

        /// <summary>
        /// 板子炮（十三张）
        /// </summary>
        [EnumAlias("十三张")]
        BanZiPao,

        /// <summary>
        /// 炸金花
        /// </summary>
        [EnumAlias("炸金花")]
        FriedGoldenFlower,
    }
}
