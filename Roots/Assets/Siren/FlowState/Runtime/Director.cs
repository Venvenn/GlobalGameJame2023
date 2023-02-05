using Shapes;

namespace Siren
{
    /// <summary>
    /// A director should hold the main flow state machine and is the flow states way of accessing unity update methods
    /// </summary>
    public abstract class Director : ImmediateModeShapeDrawer
    {
        public FlowStateMachine m_flowStateMachine = new FlowStateMachine();
    
        private void Start()
        {
            OnStart();
        }

        // Update is called once per frame
        void Update()
        {
            OnUpdate();
            m_flowStateMachine.Update();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
            m_flowStateMachine.FixedUpdate();
        }

        public abstract void OnStart();
    
        public abstract void OnUpdate();
    
        public abstract void OnFixedUpdate();
    }
}

