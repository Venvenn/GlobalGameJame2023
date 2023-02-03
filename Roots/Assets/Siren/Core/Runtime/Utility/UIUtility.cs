using UnityEngine;

namespace Siren
{
    public static class UIUtility
    {
        public static void DestroyChildren(Transform transform)
        {
            foreach (Transform child in transform) Object.Destroy(child.gameObject);
        }
    }
}