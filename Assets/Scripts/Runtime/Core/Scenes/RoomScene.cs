using UniVue;
using UniVue.View;
using UniVue.View.Views;

namespace HappyCard
{
    public sealed class RoomScene : IScene
    {
        public void Initialize()
        {
            //1. 加载资源
            RoomSceneAsset asset = AssetLoader.LoadAssetRef<RoomSceneAsset>();

            //2. 加载视图
            Vue.LoadViews(asset.SceneConfig);

            //3. 注册UI事件
            Vue.AutowireAndUnloadEventCalls(nameof(GameScenes.Room));

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
            RoomInfo room = GameDataContainer.Instance.Room;

            router.GetView(nameof(GameUIs.RoomView)).BindModel(room);
            router.GetView<ListView>(nameof(GameUIs.PlayersView)).BindList(room.Players);

        }

        private void RegisterSyncHandlers()
        {
            NetworkManager.Instance.AddHandler(nameof(SyncEvents.StartGame), new StartGameHandler())
                                   .AddHandler(nameof(SyncEvents.DestroyRoom), new DestroyRoomHandler())
                                   .AddHandler(nameof(SyncEvents.QuitRoom), new QuitRoomHandler());
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
