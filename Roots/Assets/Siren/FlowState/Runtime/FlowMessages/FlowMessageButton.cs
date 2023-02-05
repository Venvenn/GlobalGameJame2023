using UnityEngine;
using UnityEngine.UI;

namespace Siren
{
    [RequireComponent(typeof(Button))]
    public class FlowMessageButton : MonoBehaviour
    {
        public FlowUIGroup m_flowGroup;
        public FlowMessage m_flowMessage;

        private Button m_button;

        void Start()
        {
            if (m_flowGroup == null)
            {
                m_flowGroup = GetComponentInParent<FlowUIGroup>();
            }

            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(SendMessage);
        }

        private void SendMessage()
        {
            m_flowGroup.SendMessage(m_flowMessage.GetMessage());
        }
    }
}