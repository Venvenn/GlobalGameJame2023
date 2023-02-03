using UnityEngine;

namespace Siren
{
    [RequireComponent(typeof(ToggleGroupAccessible))]
    public class FlowMessageToggleGroup : MonoBehaviour
    {
        public FlowUIGroup m_flowGroup;

        private ToggleGroupAccessible m_toggleGroup;

        void Start()
        {
            if (m_flowGroup == null)
            {
                m_flowGroup = FindObjectOfType<FlowUIGroup>();
            }

            m_toggleGroup = GetComponent<ToggleGroupAccessible>();

            foreach (var toggle in m_toggleGroup.GetAllToggles())
            {
                toggle.onValueChanged.AddListener(SendMessage);
            }
        }

        private void SendMessage(bool active)
        {
            if (active)
            {
                FlowMessage flowMessage = m_toggleGroup.GetFirstActiveToggle().GetComponent<FlowMessage>();
                m_flowGroup.SendMessage(flowMessage.GetMessage());
            }
        }
    }
}