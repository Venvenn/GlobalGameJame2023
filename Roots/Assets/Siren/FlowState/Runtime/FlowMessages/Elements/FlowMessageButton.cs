using UnityEngine;
using UnityEngine.UI;

namespace Siren
{
    [RequireComponent(typeof(Button))]
    public class FlowMessageButton : MonoBehaviour
    {
        private Button m_button;
        public FlowUIGroup FlowGroup;
        public IFlowMessage FlowMessage;

        private void Start()
        {
            if (FlowGroup == null) FlowGroup = FindObjectOfType<FlowUIGroup>();

            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(SendMessage);
        }

        private void SendMessage()
        {
            FlowGroup.SendMessage(FlowMessage.GetMessage());
        }
    }
}