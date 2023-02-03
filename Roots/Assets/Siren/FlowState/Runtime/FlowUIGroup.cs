using UnityEngine;

namespace Siren
{
    /// <summary>
    /// Used to associate UI and ui elements with a FSM
    /// </summary>
    public class FlowUIGroup : MonoBehaviour
    {
        private FlowStateMachine m_connectedFSM;

        public void AttachFSM(FlowStateMachine fsm)
        {
            m_connectedFSM = fsm;
        }

        public void SendMessage(object message)
        {
            m_connectedFSM.SendFlowMessage(message);
        }
    }
}