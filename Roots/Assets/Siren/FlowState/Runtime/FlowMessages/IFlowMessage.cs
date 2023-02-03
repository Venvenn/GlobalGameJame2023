namespace Siren
{
    /// <summary>
    ///     Interface for a simple flow message that can be used to communicate between flowstates
    /// </summary>
    public interface IFlowMessage
    {
        public object GetMessage();
    }
}