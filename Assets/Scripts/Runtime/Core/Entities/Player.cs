using UnityEngine;
using UniVue.Model;

namespace HappyCard
{
    public sealed partial class Player
    {
        [AutoNotify] private int _ID;               //ID
        [AutoNotify] private string _name;          //昵称
        [AutoNotify] private int _level;            //等级
        [AutoNotify] private int _likes;            //点赞数
        [AutoNotify] private int _Exp;              //经验值
        [AutoNotify] private int _coin;             //金币
        [AutoNotify] private int _diamond;          //钻石
        [AutoNotify] private int _HP;               //体力
        [AutoNotify] private Sprite _headIcon;      //头像

        public string HeadIconID { get; set; } //根据这个来加载头像图片
    }
}
