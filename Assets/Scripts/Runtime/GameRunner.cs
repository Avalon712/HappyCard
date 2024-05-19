using HappyCard.Entities;
using HappyCard.Entities.Configs;
using HappyCard.Enums;
using HappyCard.Managers;
using HappyCard.Network;
using HappyCard.Network.Entities;
using HappyCard.Utils;
using HayypCard;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniVue;
using UniVue.Utils;
using UniVue.View.Views.Flexible;
using YooAsset;

namespace HappyCard
{
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public sealed class GameRunner : UnitySingleton<GameRunner>
    {
        [Header("��Ϸ����ģʽ")]
        public EPlayMode playMode = EPlayMode.EditorSimulateMode;


        //--------------------------------------------------------------------------------------
        private void Awake()
        {
            DontDestroyOnLoad(this);

            //���һ����ʼ��Http����������
            gameObject.AddComponent<GameHttpService>();

            //ϵͳ����
#if !UNITY_EDITOR
            Application.runInBackground = true;
            Application.targetFrameRate = 60;
#endif
            //��ʼ��Vue
            Vue.Initialize(new VueConfig() { MaxHistoryRecord=15});

            //��ʼ��YooAsset
            YooAssets.Initialize();

            //�����Զ�װ��EventCall
#if !ENABLE_IL2CPP
            //֧�ַ���
            Vue.AutowireEventCalls();
#else       //il2cpp��AOT����
            //��֧�ַ���
            Vue.Instance.BuildAutowireInfos(typeof(LoginHandler), typeof(GameUpdateHandler));
#endif

            StartCoroutine(OnEnterGameApp());
        }

        //------------------------------------�ս�����Ϸ���еĳ�ʼ��---------------------------------------

        #region ������Ϸ
        private IEnumerator OnEnterGameApp()
        {
            //������ͼ
            using(var it = GameObjectFindUtil.BreadthFind(GameObject.Find("Canvas"), nameof(GameUI.LoadView), nameof(GameUI.EnsureTipView)).GetEnumerator())
            {
                while (it.MoveNext())
                {
                    if(it.Current.name == nameof(GameUI.LoadView)) { new FlexibleView(it.Current); }
                    else if(it.Current.name == nameof(GameUI.EnsureTipView)) { new FEnsureTipView(it.Current); }
                }
            }

            //��������ʾ������Ϣ
            LoadInfo loadInfo = new LoadInfo();
            loadInfo.Bind(nameof(GameUI.LoadView));

            //�����Ϸ����
            loadInfo.message = "�����Ϸ������";
            GameHttpService httpService = GetComponent<GameHttpService>();
            yield return httpService.AsyncSendGetRequest(CheckGameUpdate(loadInfo));

            //���س�����Դ
            loadInfo.message = "������Դ������";
            //�ȼ��ع�����Դ
            using(var it = AssetManager.Instance.AsyncLoadPublicAsset().GetEnumerator())
            {
                while (it.MoveNext())
                {
                    var op = it.Current;
                    while (!op.IsDone) { yield return null; }
                }
            }

            loadInfo.message = "���ش浵���ݼ�����";
            yield return GameDataManager.Instance.AsyncLoadLocalArchiveData();

            loadInfo.message = "���ش浵���ݼ������";
            loadInfo.Unbind(); //������ݰ�

            //��ʼ����������������������
            NetworkManager.Instance.SetGameNetworkServiceMode(GameDataManager.Instance.NetworkInfo.mode);

            //ǰ�ڳ�ʼ�������ע���¼� 
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            //���������¼�
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        #endregion

        #region �����Ϸ����
        /// <summary>
        /// �����Ϸ����
        /// </summary>
        /// <returns>��Ϸ�Ƿ���Ҫ���и���</returns>
        public HttpInfo<GameAppInfo, GameAppInfo> CheckGameUpdate(LoadInfo loadInfo)
        {
            HttpInfo<GameAppInfo, GameAppInfo> httpInfo = new(GameEvent.CheckUpdate);

            //�������
            httpInfo.onProcess = (process) => loadInfo.process = process * 100;

            //����ɹ�
            httpInfo.onSuccessed = () =>
            {
                //�汾�ȶ�
                if (httpInfo.responseData.version != GameDataManager.Instance.GameAppInfo.version)
                {
                    string message = "��鵽��Ϸ����,��Ҫ���и��²��ܽ�����Ϸ,���ȡ�����˳���Ϸ";
                    Vue.Router.GetView<FEnsureTipView>(nameof(GameUI.EnsureTipView)).Open("��Ϸ����",message, GameUpdate, QuitGameApp);
                }

                GameDataManager.Instance.NetworkInfo.mode = NetworkServiceMode.WAN;
            };

            httpInfo.onFailed = () => GameDataManager.Instance.NetworkInfo.mode = NetworkServiceMode.LAN;

            return httpInfo;
        }
#endregion

        #region ��Ϸ����
        public void GameUpdate()
        {
            Vue.Router.Close(nameof(GameUI.EnsureTipView)); //�ر���ʾ���µ���ͼ
        }
        #endregion

        #region �˳���Ϸ
        public void QuitGameApp()
        {
            Application.Quit();
        }
        #endregion

        //------------------------------------�����������--------------------------------------------------
        private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
        {
            //GameScene gameScene = (GameScene)scene.buildIndex; //emmmm������˳��һ�»ᵼ��bug....
            GameScene gameScene = Enum.Parse<GameScene>(scene.name); 
            StartCoroutine(AsyncInitializeScene(gameScene));
        }

        private IEnumerator AsyncInitializeScene(GameScene gameScene)
        {
            if(gameScene != GameScene.Login)
            {
                //��õ�ǰ�����µ�LoadView
                GameObject viewObject = GameObjectFindUtil.BreadthFind(nameof(GameUI.LoadView), GameObject.Find("Canvas"));
                new FlexibleView(viewObject);
            }
            
            LoadInfo loadInfo = new LoadInfo();
            loadInfo.Bind(nameof(GameUI.LoadView));

            loadInfo.message = "����������"; 
            //���س�����ͼ
            var operation = AssetManager.Instance.AsyncLoadScneViews(gameScene);

            while (!operation.IsDone)
            {
                yield return null;
            }

            //���س�����ͼ
            Vue.LoadViews(AssetManager.Instance.GetGameSceneViewConfig(gameScene));

            //����ÿ������������ʲ�
            using(var it = AssetManager.Instance.AsyncLoadScenExtraAsset(gameScene).GetEnumerator())
            {
                while (it.MoveNext())
                {
                    var op = it.Current;
                    while (!op.IsDone)
                    {
                        yield return null;
                    }
                }
            }

            //װ�䳡���µ�EventCall
            Vue.AutowireAndUnloadEventCalls(gameScene.ToString());

            loadInfo.message = "�������";

            //����
            //OutputAllLoadedViews();

            //׼����Ϸ
            GameDataBind.Instance.PrepareGameDataAfterSceneLoaded(gameScene);
        }


        //--------------------------------------����ж�����------------------------------------------------
        private void OnSceneUnloaded(Scene scene)
        {
            GameScene gameScene = (GameScene)scene.buildIndex;

            //�����һ������ע��������¼�
            NetworkManager.Instance.ClearHandles();

            //�ͷ���ͼ��Դ
            Vue.UnloadCurrentSceneResources();

            //ж�س�����Դ
            AssetManager.Instance.UnloadSceneAsset(gameScene);

            //�ͷ���Ϸ������Դ
            GameDataBind.Instance.DisposeGameDataAfterSceneUnloaded(gameScene);
        }

        //---------------------------------------------------------------------------------------------------

        private void OutputAllLoadedViews()
        {
            using(var it = Vue.Router.GetAllView().GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Debug.Log(it.Current.name);
                }
            }
        }
    }
}
