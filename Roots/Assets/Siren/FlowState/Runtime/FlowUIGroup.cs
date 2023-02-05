using UnityEngine;

namespace Siren
{
    /// <summary>
    /// A flow ui group is used to send flow messages attached to ui to flow states 
    /// </summary>
    public class FlowUIGroup : MonoBehaviour
    {
        private FlowState m_connectedFlowState;

        public void AttachFlowState(FlowState flowState)
        {
            m_connectedFlowState = flowState;
        }

        public void SendMessage(object message)
        {
            m_connectedFlowState.SendFlowMessage(message, m_connectedFlowState);
        }
    }
}