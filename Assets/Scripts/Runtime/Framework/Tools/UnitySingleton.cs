using UnityEngine;

namespace HappyCard
{
    public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
    {
        private static T _instanced;

        /// <summary>
        /// 指示当前单例对象为全局还是场景
        /// </summary>
        public virtual Lifecycle Lifecycle { get; } = Lifecycle.Global;

        public static T Instance
        {
            get
            {
                if (_instanced == null)
                {
                    _instanced = new GameObject("Singleton_" + typeof(T).Name).AddComponent<T>();
                    if (_instanced.Lifecycle == Lifecycle.Global)
                        DontDestroyOnLoad(_instanced);
                }
                return _instanced;
            }
        }

        /// <summary>
        /// 销毁单例对象
        /// </summary>
        public void Destroy()
        {
            if (_instanced.Lifecycle == Lifecycle.Scene)
            {
                Destroy(_instanced);
                _instanced = null;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("无法销毁一个生命周期为全局单例的对象");
            }
#endif
        }
    }

    public enum Lifecycle
    {
        Global,
        Scene,
    }
}
