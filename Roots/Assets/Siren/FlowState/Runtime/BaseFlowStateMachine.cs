using System.Collections.Generic;

namespace Siren
{
    /// <summary>
    /// State machine that control the flow between states as well as what stage a state is in 
    /// </summary>
    public abstract class BaseFlowStateMachine
    {
        protected enum StackAction
        {
            PUSH,
            POP
        }

        /// <summary>
        /// Used to signify that this flow state machine is a sub state machine residing inside a differnt state
        /// </summary>
        public FlowState m_owningState = null;

        /// <summary>
        /// queue used to exexute actions in order, so that state actions are accurately performed in the order they are intended to be
        /// </summary>
        protected Queue<(StackAction, FlowState)> m_stateActionQueue = new Queue<(StackAction, FlowState)>();
        
        /// <summary>
        /// messages sent by ui or other states that will be delivered to the active state
        /// </summary>
        protected Queue<(FlowState state,object message)> m_flowMessages = new Queue<(FlowState state,object message)>();

        /// <summary>
        /// update the state on the top of the state stack
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// performs a fixed update for the top state
        /// </summary>
        public abstract void FixedUpdate();
        
        /// <summary>
        /// draws Shapes
        /// </summary>
        public abstract void Render();
        
        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        public abstract void Pop();

        /// <summary>
        /// pushes a new state onto the top of the stack 
        /// </summary>
        public abstract void Push(FlowState flowState);

        /// <summary>
        /// returns the currently active flow state
        /// </summary>
        public abstract FlowState GetTopState();

        /// <summary>
        /// Pops all of the states belonging to this state machine
        /// </summary>
        public abstract void PopAllStates();
        
        /// <summary>
        /// adds a message to the message queue to be sent to the top state
        /// </summary>
        public void SendFlowMessage(object message, FlowState state)
        {
            m_flowMessages.Enqueue((state, message));
        }

        /// <summary>
        /// Sends all the messages in the message queue to the top state
        /// </summary>
        protected abstract void ReceiveFlowMessages();

        /// <summary>
        /// performs stack actions if there are any to perform
        /// </summary>
        protected abstract void UpdateStateStack();

        /// <summary>
        /// pushes a new state onto the top of the stack 
        /// </summary>
        protected abstract void PushState(FlowState flowState);

        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        protected abstract void PopState();
    }
}