using System.Collections.Generic;
using UnityEngine;
using UniVue;
using UniVue.Model;
using UniVue.Tween;
using UniVue.View;
using UniVue.ViewModel;
using UniVue.ViewModel.Models;

namespace HappyCard
{
    public sealed class MainScene : IScene
    {
        public void Initialize()
        {
            //1. 加载资源
            MainSceneAsset asset = AssetLoader.LoadAssetRef<MainSceneAsset>();

            //2. 加载视图
            Vue.LoadViews(asset.SceneConfig);

            //3. 注册UI事件
            Vue.AutowireAndUnloadEventCalls(nameof(GameScenes.Main));

            //4. 绑定数据到视图
            BindDataToViews();

            //5. 注册网络事件
            RegisterSyncHandlers();

            //6. 卸载不用的资源
            asset.DestroyRef();
        }


        private void BindDataToViews()
        {
            ViewRouter router = Vue.Router;
            Player self = GameDataContainer.Instance.Self;
            GameSetting setting = GameDataContainer.Instance.GameSetting;

            router.GetView(nameof(GameUIs.MainView)).BindModel(self);
            router.GetView(nameof(GameUIs.GameplayView)).BindModel(setting);
            router.GetView(nameof(GameUIs.CreateRoomView)).BindModel(setting);
        }


        private void RegisterSyncHandlers()
        {

        }


        public void Dispose()
        {
            //1. 卸载视图资源
            Vue.UnloadCurrentSceneResources();

            //2. 清空当前场景的同步处理器
            NetworkManager.Instance.ClearHandlers();
        }
    }
}
