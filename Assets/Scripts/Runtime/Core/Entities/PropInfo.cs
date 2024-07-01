using UnityEngine;
using UniVue.Model;

namespace HappyCard
{
    public sealed partial class PropInfo : ScriptableObject
    {
        [AutoNotify][SerializeField] private Sprite _icon;      //道具图标
        [AutoNotify][SerializeField] private string _shortInfo; //道具简短描述
    }
}
