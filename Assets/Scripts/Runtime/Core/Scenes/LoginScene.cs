using UniVue;

namespace HappyCard
{
    public sealed class LoginScene : IScene
    {
        public void Initialize()
        {
            //1. 加载资源
            LoginSceneAsset asset = AssetLoader.LoadAssetRef<LoginSceneAsset>();

            //2. 加载视图
            Vue.LoadViews(asset.SceneConfig);

            //3. 注册UI事件
            Vue.AutowireAndUnloadEventCalls(nameof(GameScenes.Login));

            //4. 卸载不用的资源
            asset.DestroyRef();

            //5. 自动登录
            LoginUI.AutoLogin();
        }

        public void Dispose()
        {
            //1. 卸载视图资源
            Vue.UnloadCurrentSceneResources();
        }
    }
}
