namespace Siren
{
    public class FlowMessageString : FlowMessage
    {
        public string m_message;

        public override object GetMessage()
        {
            return m_message;
        }
    }
}