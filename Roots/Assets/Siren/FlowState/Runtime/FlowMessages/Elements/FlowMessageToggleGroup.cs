using UnityEngine;

namespace Siren
{
    [RequireComponent(typeof(ToggleGroupAccessible))]
    public class FlowMessageToggleGroup : MonoBehaviour
    {
        public FlowUIGroup m_flowGroup;

        private ToggleGroupAccessible m_toggleGroup;

        private void Start()
        {
            if (m_flowGroup == null) m_flowGroup = FindObjectOfType<FlowUIGroup>();

            m_toggleGroup = GetComponent<ToggleGroupAccessible>();

            foreach (var toggle in m_toggleGroup.GetAllToggles()) toggle.onValueChanged.AddListener(SendMessage);
        }

        private void SendMessage(bool active)
        {
            if (active)
            {
                IFlowMessage flowMessage = m_toggleGroup.GetFirstActiveToggle().GetComponent<IFlowMessage>();
                m_flowGroup.SendMessage(flowMessage.GetMessage());
            }
        }
    }
}