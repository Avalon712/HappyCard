using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using UniVue;
using UniVue.View.Config;
using Random = UnityEngine.Random;

namespace HappyCard
{
    public class Local_FightLandlord_Test : MonoBehaviour, ICmdListener
    {
        public SpriteAtlas _pokerCardIcons;
        public SceneConfig _sceneConfig;
        public Sprite _winIcon;
        public Sprite _loseIcon;

#if UNITY_EDITOR
        [ContextMenu("RuleCheck")]
        private void RuleCheck()
        {
            //Json
            //List<PokerCard>[] cards = new List<PokerCard>[2]
            //{
            //    new List<PokerCard>(){PokerCard.Club_6, PokerCard.Diamond_A},
            //    new List<PokerCard>(){PokerCard.Spade_3, PokerCard.Club_3, PokerCard.Heart_4}
            //};
            //string json = JsonConvert.SerializeObject(cards);
            //Debug.Log(json);
            //List<PokerCard>[] cards2 = JsonConvert.DeserializeObject<List<PokerCard>[]>(json);
            //Debug.Log(cards2[0].Capacity);
            //Debug.Log(cards2[1].Capacity);
            //Debug.Log(JsonConvert.SerializeObject(cards2));

            //测试牌数一致时但是当前出牌为炸弹时的情况
            //List<PokerCard> lastOutCards = new List<PokerCard>() { PokerCard.Club_Q, PokerCard.Diamond_Q, PokerCard.Spade_Q, PokerCard.Spade_4 };
            //PokerType lastType = PokerType.ThreeWithOne;
            //List<PokerCard> bombs = new List<PokerCard>() { PokerCard.Spade_3, PokerCard.Heart_3, PokerCard.Club_3, PokerCard.Diamond_3 };
            //Debug.Log(new FightLandlordHelper().FastCheck(bombs, lastOutCards, lastType, out PokerType currentType) + " " + currentType);

            //测试连对与飞机不带的检查
            //List<PokerCard> cards = new List<PokerCard>() {
            //    PokerCard.Club_10, PokerCard.Diamond_10, PokerCard.Spade_10,
            //    PokerCard.Heart_J, PokerCard.Diamond_J, PokerCard.Spade_J
            //};
            //Debug.Log(new FightLandlordHelper().GetPokerType(cards));

            //测试四带2与飞机带2的检查
            //List<PokerCard> fj2 = new List<PokerCard>() {
            //    PokerCard.Club_6, PokerCard.Diamond_6, PokerCard.Spade_6,
            //    PokerCard.Heart_7, PokerCard.Diamond_7, PokerCard.Spade_7,
            //    PokerCard.Club_3, PokerCard.Club_9
            //};
            //List<PokerCard> f2 = new List<PokerCard>() {
            //    PokerCard.Heart_J, PokerCard.Diamond_J, PokerCard.Spade_J, PokerCard.Club_J,
            //    PokerCard.Club_10, PokerCard.Club_K
            //};

            //Debug.Log(new FightLandlordHelper().FastCheck(f2, fj2, PokerType.AeroplaneWithTwo, out PokerType currentType) + " " + currentType);
        }
#endif


        private void Awake()
        {
            Vue.Initialize(VueConfig.Create());
        }

        private void Start()
        {
            //加载视图
            Vue.LoadViews(_sceneConfig);

            //using (var it = Vue.Router.GetAllView().GetEnumerator())
            //{
            //    while (it.MoveNext())
            //    {
            //        //if (it.Current.name == "FightLandlord_BoutPhaseView")
            //        //{
            //        //    using (var itt = it.Current.GetNestedViews().GetEnumerator())
            //        //    {
            //        //        while (itt.MoveNext())
            //                    //Debug.Log(it.Current.name);
            //        //    }
            //        //}
            //    }
            //}

            //List<UIEvent> uIEvents = Vue.Event.GetType().GetField("_events", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Vue.Event) as List<UIEvent>;
            //for (int i = 0; i < uIEvents.Count; i++)
            //{
            //    Debug.Log(uIEvents[i].ViewName + " ---> " + uIEvents[i].EventName);
            //}

            InitTestSceneData();

            CmdExecutor.Instance.AddListener(this);
        }

        private void InitTestSceneData()
        {
            List<Player> players = new List<Player>(3);
            for (int i = 0; i < 3; i++)
            {
                players.Add(new Player() { ID = i, Name = "Test" + i });
            }
            GameDataContainer.Instance.Self = players[0];

            GameDataContainer.Instance.Room = new RoomInfo()
            {
                OwnerID = 0,
                Players = new UniVue.Utils.ObservableList<Player>(players)
            };

            List<ValueTuple<Player, IVirtualPlayer>> virtualPlayers = new List<(Player, IVirtualPlayer)>(3);

            GameLoop gameLoop = new GameLoop(Gameplay.FightLandlord, new FightLandlordHelper(), virtualPlayers);
            GameDataContainer.Instance.Loop = gameLoop;

            for (int i = 0; i < 3; i++)
            {
                IVirtualPlayer virtualPlayer = new LocalPlayer(players[i].ID, i != 0, 5);
                virtualPlayers.Add((players[i], virtualPlayer));

                if (i == 0)
                {
                    virtualPlayer.AddBoutListener(new SelfUI())
                                 .AddBoutListener(new FightLandlord_BoutPhaseUI(nameof(GameUIs.FightLandlord_BoutPhaseView)))
                                 .AddBoutListener(new CardsUI());
                }
                else
                {
                    string viewName = i == 1 ? nameof(GameUIs.FightLandlord_LeftPlayerView) :
                                               nameof(GameUIs.FightLandlord_RightPlayerView);
                    virtualPlayer.AddBoutListener(new OthersUI(players[i], viewName));
                }
            }

            GameOverUI gameOverUI = new GameOverUI(_winIcon, _loseIcon);
            GameTopInfoUI topInfoUI = new GameTopInfoUI();

            gameLoop.AddListener(gameOverUI)
                    .AddListener(topInfoUI);

            CmdExecutor.Instance.AddListener(gameOverUI)
                                .AddListener(topInfoUI);

            Dictionary<PokerCard, CardInfo> pokerCards = new Dictionary<PokerCard, CardInfo>();
            Sprite[] icons = new Sprite[_pokerCardIcons.spriteCount];
            _pokerCardIcons.GetSprites(icons);
            Type type = typeof(PokerCard);
            string[] cardNames = Enum.GetNames(type);
            for (int i = 0; i < cardNames.Length; i++)
            {
                PokerCard pokerCard = Enum.Parse<PokerCard>(cardNames[i]);
                CardInfo cardInfo = new CardInfo(pokerCard, Array.Find(icons, icon => icon.name.StartsWith(cardNames[i])));
                pokerCards.Add(pokerCard, cardInfo);
            }
            GameDataContainer.Instance.PokerCards = pokerCards;

            GameSetting gameSetting = new GameSetting() { ShuffleMode = ShuffleMode.NoShuffle, Timer = 30 };
            GameDataContainer.Instance.GameSetting = gameSetting;
        }

        public void BeforeCmdExecute(ICommand cmd)
        {
            if (cmd is PrepareCmd)
                GameDataContainer.Instance.Loop.StartLoop();
        }
    }
}
