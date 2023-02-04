using UnityEngine;

namespace Siren
{
    /// <summary>
    /// A flow message can be sent to a flow state and contain any object, a good way to communicate between states or for ui events
    /// a flow message does not need to inherit from this class if it does not need to be attached to a mono behaviour
    /// </summary>
    public abstract class FlowMessage : MonoBehaviour
    {
        public abstract object GetMessage();
    }
}