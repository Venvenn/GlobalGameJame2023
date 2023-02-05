namespace Siren
{
    public class FlowMessageInt : FlowMessage
    {
        public int m_message;

        public override object GetMessage()
        {
            return this;
        }
    }
}