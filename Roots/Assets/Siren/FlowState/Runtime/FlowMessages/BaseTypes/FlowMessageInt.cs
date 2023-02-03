namespace Siren
{
    public class FlowMessageInt : IFlowMessage
    {
        private int m_message;

        public object GetMessage()
        {
            return m_message;
        }
    }
}