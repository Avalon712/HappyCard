using UnityEngine;

namespace HappyCard
{
    public static class AssetLoader
    {
        public static T LoadAssetRef<T>() where T : AssetRef
        {
            return GameObject.Find(nameof(AssetRef))?.GetComponent<T>();
        }
    }
}
