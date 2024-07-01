using UnityEngine;
using UniVue;
using UniVue.View.Config;

namespace HappyCard
{
    public class SceneViewsLoadTest : MonoBehaviour
    {
        public SceneConfig _sceneConfig;

        private void Awake()
        {
            Vue.Initialize(VueConfig.Create());
        }

        private void Start()
        {
            Vue.LoadViews(_sceneConfig);
        }
    }
}
