namespace Siren
{
    /// <summary>
    /// Base class you should inherit your UI screens from
    /// </summary>
    public abstract class FlowScreenUI
    {
        public abstract void Init();
        public abstract void Update();
        public abstract void Destroy();
    }
}