using UnityEngine;
using UniVue.View.Config;

namespace HappyCard
{
    public sealed class MainSceneAsset : AssetRef
    {
        [SerializeField] private SceneConfig _sceneConfig;

        public SceneConfig SceneConfig => _sceneConfig;

    }
}
