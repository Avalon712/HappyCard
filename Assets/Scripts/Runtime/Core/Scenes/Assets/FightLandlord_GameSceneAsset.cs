using UnityEngine;
using UnityEngine.U2D;
using UniVue.View.Config;

namespace HappyCard
{
    public sealed class FightLandlord_GameSceneAsset : AssetRef
    {
        [SerializeField] private SceneConfig _sceneConfig;
        [SerializeField] private SpriteAtlas _pokerIcons;
        [SerializeField] private Sprite _winIcon;
        [SerializeField] private Sprite _loseIcon;

        public SceneConfig SceneConfig => _sceneConfig;

        public SpriteAtlas PokerIcons => _pokerIcons;

        public Sprite WinIcon => _winIcon;

        public Sprite LoseIcon => _loseIcon;
    }
}
