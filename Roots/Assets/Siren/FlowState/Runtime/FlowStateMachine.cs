using System.Collections.Generic;

namespace Siren
{
    /// <summary>
    /// State machine that control the flow between states as well as what stage a state is in 
    /// </summary>
    public class FlowStateMachine : BaseFlowStateMachine
    {
        /// <summary>
        /// the state stack is used to store all the states owned by this fsm, the top one being the currently active state
        /// </summary>
        private Stack<FlowState> m_stateStack = new Stack<FlowState>();

        public int StateCount => m_stateStack.Count;
        
        public FlowStateMachine(FlowState owningState = null)
        {
            m_owningState = owningState;
        }
        
        /// <summary>
        /// update the state on the top of the state stack
        /// </summary>
        public override void Update()
        {
            if (m_stateStack.Count > 0)
            {
                FlowState flowState = m_stateStack.Peek();
                switch (flowState.m_stage)
                {
                    case FlowState.StateStage.PRESENTING:
                    {
                        FlowState.TransitionState transitionState = flowState.UpdateInitialise();
                        if (transitionState == FlowState.TransitionState.COMPLETED)
                        {
                            flowState.FinishInitialise();
                            flowState.OnActive();
                            flowState.m_stage = FlowState.StateStage.ACTIVE;
                        }

                        break;
                    }
                    case FlowState.StateStage.ACTIVE:
                    {
                        ReceiveFlowMessages();
                        flowState.ActiveUpdate();
                        break;
                    }
                    case FlowState.StateStage.DISMISSING:
                    {
                        FlowState.TransitionState transitionState = flowState.UpdateDismiss();
                        if (transitionState == FlowState.TransitionState.COMPLETED)
                        {
                            flowState.FinishDismiss();
                            flowState.m_stage = FlowState.StateStage.INACTIVE;
                            PopState();
                        }

                        break;
                    }
                }
            }
            UpdateStateStack();
        }

        /// <summary>
        /// performs a fixed update for the top state
        /// </summary>
        public override void FixedUpdate()
        {
            if (m_stateStack.Count > 0 && m_stateStack.Peek().m_stage == FlowState.StateStage.ACTIVE)
            {
                m_stateStack.Peek().ActiveFixedUpdate();
            }
        }

        public override void Render()
        {
            if (m_stateStack.Count > 0 && m_stateStack.Peek().m_stage == FlowState.StateStage.ACTIVE)
            {
                m_stateStack.Peek().OnRender();
            }
        }

        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        public override void Pop()
        {
            m_stateActionQueue.Enqueue((StackAction.POP, null));
        }

        /// <summary>
        /// pushes a new state onto the top of the stack 
        /// </summary>
        public override void Push(FlowState flowState)
        {
            m_stateActionQueue.Enqueue((StackAction.PUSH, flowState));
        }
        
        /// <summary>
        /// returns the currently active flow state
        /// </summary>
        public override FlowState GetTopState()
        {
            if (m_stateStack.Count > 0)
            {
                return m_stateStack.Peek();
            }

            return null;
        }

        /// <summary>
        /// Sends all the messages in the message queue to the top state
        /// </summary>
        protected override void ReceiveFlowMessages()
        {
            for (int i = 0; i < m_flowMessages.Count; i++)
            {
                object message = m_flowMessages.Dequeue().message;
                m_stateStack.Peek().ReceiveFlowMessages(message);
            }
        }
        
        /// <summary>
        /// performs stack actions if there are any to perform
        /// </summary>
        protected override void UpdateStateStack()
        {
            if (m_stateActionQueue.Count > 0)
            {
                var action = m_stateActionQueue.Dequeue();
                switch (action.Item1)
                {
                    case StackAction.POP:
                        if (m_stateStack.Count > 0)
                        {
                            m_stateStack.Peek().OnDismiss();
                            m_stateStack.Peek().m_stage = FlowState.StateStage.DISMISSING;
                        }
                        break;
                    case StackAction.PUSH:
                        PushState(action.Item2);
                        break;
                }
            }
        }

        /// <summary>
        /// pushes a new state onto the top of the stack 
        /// </summary>
        protected override void PushState(FlowState flowState)
        {
            if (m_stateStack.Count > 0)
            {
                m_stateStack.Peek().m_stage = FlowState.StateStage.INACTIVE;
                m_stateStack.Peek().OnInactive();
            }

            m_stateStack.Push(flowState);

            flowState.FlowStateMachine = this;
            flowState.m_stage = FlowState.StateStage.PRESENTING;
            flowState.OnInitialise();
        }
        
        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        protected override void PopState()
        {
            if (m_stateStack.Count > 0)
            {
                m_stateStack.Peek().OnInactive();
                m_stateStack.Pop();

                if (m_stateStack.Count > 0)
                {
                    m_stateStack.Peek().OnActive();
                    m_stateStack.Peek().m_stage = FlowState.StateStage.ACTIVE;
                }
            }
        }
        
        /// <summary>
        /// Pops all of the states belonging to this state machine
        /// </summary>
        public override void PopAllStates()
        {
            for (int i = 0; i < m_stateStack.Count; i++)
            {
                Pop();
            }
        }
    }
}