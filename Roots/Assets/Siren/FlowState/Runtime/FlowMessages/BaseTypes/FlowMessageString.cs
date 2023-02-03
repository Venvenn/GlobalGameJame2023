namespace Siren
{
    public class FlowMessageString : IFlowMessage
    {
        private string m_message;

        public object GetMessage()
        {
            return m_message;
        }
    }
}