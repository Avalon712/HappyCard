using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UniVue;

namespace HappyCard
{
    public sealed class FightLandlord_GameScene : IScene
    {
        public void Initialize()
        {
            //1. 加载资源
            FightLandlord_GameSceneAsset asset = AssetLoader.LoadAssetRef<FightLandlord_GameSceneAsset>();

            //2. 加载视图
            Vue.LoadViews(asset.SceneConfig);

            //3. 创建游戏循环
            CreateGameLoop();

            //4. 注册命令执行监听器事件
            RegisterListeners(asset.WinIcon, asset.LoseIcon);

            //5. 注册网络事件
            RegisterSyncHandlers();

            //6. 注册游戏回合监听器
            RegisterBoutListeners();

            //7. 绑定数据到视图
            BindDataToViews();

            //8. 创建扑克牌
            CreatePokerCards(asset.PokerIcons);

            //9. 卸载不用的资源
            asset.DestroyRef();
        }


        private void CreateGameLoop()
        {
            RoomInfo room = GameDataContainer.Instance.Room;
            GameDataContainer.Instance.Loop = new GameLoop(room.Gameplay, new FightLandlordHelper(), CreateVirtualPlayers(room));
        }


        private void RegisterBoutListeners()
        {
            List<ValueTuple<Player, IVirtualPlayer>> vPlayers = GameDataContainer.Instance.Loop.VirtualPlayers;
            RoomInfo room = GameDataContainer.Instance.Room;

            int selfIndex = room.Players.IndexOf(GameDataContainer.Instance.Self); 
            int leftIndex = (selfIndex + 1) % vPlayers.Count;
            int rightIndex = (leftIndex + 1) % vPlayers.Count;

            vPlayers[selfIndex].Item2.AddBoutListener(new SelfUI())
                             .AddBoutListener(new FightLandlord_BoutPhaseUI(nameof(GameUIs.FightLandlord_BoutPhaseView)))
                             .AddBoutListener(new CardsUI());

            vPlayers[leftIndex].Item2.AddBoutListener(new OthersUI(vPlayers[leftIndex].Item1, nameof(GameUIs.FightLandlord_LeftPlayerView)));
            vPlayers[rightIndex].Item2.AddBoutListener(new OthersUI(vPlayers[rightIndex].Item1, nameof(GameUIs.FightLandlord_RightPlayerView)));
        }


        private void RegisterListeners(Sprite winIcon, Sprite loseIcon)
        {
            GameOverUI gameOverUI = new GameOverUI(winIcon, loseIcon);
            GameTopInfoUI topInfoUI = new GameTopInfoUI();

            GameDataContainer.Instance.Loop.AddListener(gameOverUI)
                                            .AddListener(topInfoUI);

            CmdExecutor.Instance.AddListener(gameOverUI)
                                .AddListener(topInfoUI);
        }


        private void BindDataToViews()
        {
            Vue.Router.GetView(nameof(GameUIs.SelfView)).BindModel(GameDataContainer.Instance.Self);
        }


        private void RegisterSyncHandlers()
        {
            RoomInfo room = GameDataContainer.Instance.Room;
            if (room.Status == HayypCard.NetworkStatus.Local) return;


        }


        private void CreatePokerCards(SpriteAtlas atlas)
        {
            Dictionary<PokerCard, CardInfo> pokerCards = new Dictionary<PokerCard, CardInfo>();
            Sprite[] icons = new Sprite[atlas.spriteCount];
            atlas.GetSprites(icons);
            Type type = typeof(PokerCard);
            string[] cardNames = Enum.GetNames(type);
            for (int i = 0; i < cardNames.Length; i++)
            {
                PokerCard pokerCard = Enum.Parse<PokerCard>(cardNames[i]);
                CardInfo cardInfo = new CardInfo(pokerCard, Array.Find(icons, icon => icon.name.StartsWith(cardNames[i])));
                pokerCards.Add(pokerCard, cardInfo);
            }
            GameDataContainer.Instance.PokerCards = pokerCards;
        }


        public void Dispose()
        {
            //1. 卸载视图资源
            Vue.UnloadCurrentSceneResources();            
            
            //2. 清空当前场景的同步处理器
            NetworkManager.Instance.ClearHandlers();

            //3. 清空命令监听器
            CmdExecutor.Instance.ClearListeners();
        }

        private List<ValueTuple<Player, IVirtualPlayer>> CreateVirtualPlayers(RoomInfo room)
        {
            if (room.Status == HayypCard.NetworkStatus.Local)
                return CreateLocalVirtualPlayers(room);
            else
                return CreateNetVirtualPlayers(room);
        }


        private List<ValueTuple<Player, IVirtualPlayer>> CreateLocalVirtualPlayers(RoomInfo room)
        {
            List<ValueTuple<Player, IVirtualPlayer>> players = new List<(Player, IVirtualPlayer)>(room.Players.Count);

            for (int i = 0; i < room.Players.Count; i++)
            {
                Player player = room.Players[i];
                players.Add((player, new LocalPlayer(player.ID, player.ID != room.OwnerID)));
            }
            return players;
        }


        private List<ValueTuple<Player, IVirtualPlayer>> CreateNetVirtualPlayers(RoomInfo room)
        {
            List<ValueTuple<Player, IVirtualPlayer>> players = new List<(Player, IVirtualPlayer)>(room.Players.Count);

            for (int i = 0; i < room.Players.Count; i++)
            {
                Player player = room.Players[i];
                players.Add((player, new NetPlayer()));
            }
            return players;
        }
    }
}
