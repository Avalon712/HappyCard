using UnityEngine;
using UniVue.View.Config;

namespace HappyCard
{
    public sealed class LoginSceneAsset : AssetRef
    {
        [SerializeField] private SceneConfig _sceneConfig;

        public SceneConfig SceneConfig => _sceneConfig;

    }
}
