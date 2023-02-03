using UnityEngine;

namespace Siren
{
    /// <summary>
    /// Base class you should inherit your UI screens from
    /// </summary>
    public abstract class FlowScreenUI : MonoBehaviour
    {
        public abstract void InitUI();
        public abstract void UpdateUI();
        public abstract void DestroyUI();
    }
}