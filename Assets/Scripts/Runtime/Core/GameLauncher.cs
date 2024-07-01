using HappyCard;
using UnityEngine;
using UniVue;

namespace HayypCard
{
    public sealed class GameLauncher : MonoBehaviour
    {
        private void Awake()
        {
            //初始化Vue
            Vue.Initialize(VueConfig.Create());
            
            //记录所有EventCall信息
            Vue.AutowireEventCalls();

            //初始化场景
            SceneController.Initialize();
        }


        private void Start()
        {
            if (GameUpdater.CheckUpdate())
            {
                GameUpdater.Update();
            }
        }


    }
}
