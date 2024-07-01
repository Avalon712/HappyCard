using UnityEngine;

namespace HappyCard
{
    public abstract class AssetRef : MonoBehaviour
    {
        public void DestroyRef()
        {
            Destroy(gameObject);
        }
    }
}
