using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UniVue;
using UniVue.View.Config;
using UnityEngine.U2D;

namespace HappyCard
{
    public class PlaybackTest : MonoBehaviour
    {
        public SpriteAtlas _pokerCardIcons;
        public SceneConfig _sceneConfig;

        [Header("GameFile文件的在Assets目录下的路径")]
        public string gameFilePath;

        private void Awake()
        {
            Vue.Initialize(VueConfig.Create());
        }

        public void Start()
        {
            Vue.LoadViews(_sceneConfig);

            GameDataContainer.Instance.Self = new Player() { ID = 0};

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

            GameFile file = JsonConvert.DeserializeObject<GameFile>(File.ReadAllText(Application.dataPath + "/" + gameFilePath));
            new PlaybackCtrlPanelUI(file);

        }

    }
}
