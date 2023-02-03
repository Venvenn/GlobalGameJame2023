using System.Collections.Generic;
using UnityEngine;

namespace Siren
{
    /// <summary>
    /// State machine that control the flow between states as well as what stage a state is in 
    /// </summary>
    public class ContinuousStateMachine : BaseFlowStateMachine
    {
        /// <summary>
        /// the state stack is used to store all the states owned by this fsm, the top one being the currently active state
        /// </summary>
        private LinkedList<FlowState> m_stateStack = new LinkedList<FlowState>();

        public int StateCount => m_stateStack.Count;
        
        public ContinuousStateMachine(FlowState owningState = null)
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
                List<FlowState> flowStatesToRemove = new List<FlowState>();
                foreach (FlowState flowState in m_stateStack)
                {
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
                                flowStatesToRemove.Add(flowState); 
                            }
                            break;
                        }
                    }
                }

                for (int i =  flowStatesToRemove.Count-1 ; i >=0; i--)
                {
                    RemoveStateFromStack(flowStatesToRemove[i]);
                }
            }

            UpdateStateStack();
        }

        /// <summary>
        /// performs a fixed update for the top state
        /// </summary>
        public override void FixedUpdate()
        {
            if (m_stateStack.Count > 0)
            {
                foreach (FlowState flowState in m_stateStack)
                {
                    switch (flowState.m_stage)
                    {
                        case FlowState.StateStage.ACTIVE:
                        {
                            flowState.ActiveFixedUpdate();
                            break;
                        };
                    }
                }
            }
        }

        public override void Render()
        {
            if (m_stateStack.Count > 0)
            {
                foreach (FlowState flowState in m_stateStack)
                {
                    switch (flowState.m_stage)
                    {
                        case FlowState.StateStage.ACTIVE:
                        {
                            flowState.OnRender();
                            break;
                        };
                    }
                }

            }
        }


        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        public override void Pop()
        {
            m_stateActionQueue.Enqueue((StackAction.POP, m_stateStack.Last.Value));
        }
        
        /// <summary>
        /// removes the state from the state stack 
        /// </summary>
        public void RemoveState(FlowState state)
        {
            m_stateActionQueue.Enqueue((StackAction.POP, state));
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
                return m_stateStack.Last.Value;
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
                (FlowState state,object message) = m_flowMessages.Dequeue();
                m_stateStack.Find(state)?.Value.ReceiveFlowMessages(message);
            }
        }
        
        /// <summary>
        /// performs stack actions if there are any to perform
        /// </summary>
        protected override void UpdateStateStack()
        {
            for (int i = 0; i < m_stateActionQueue.Count; i++)
            {
                (StackAction action, FlowState state) = m_stateActionQueue.Dequeue();
                if (state != null)
                {
                    switch (action)
                    {
                        case StackAction.POP:
                            state.OnDismiss();
                            state.m_stage = FlowState.StateStage.DISMISSING;
                            break;
                        case StackAction.PUSH:
                            PushState(state);
                            break;
                    }
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
                flowState.m_stage = FlowState.StateStage.INACTIVE;
                flowState.OnInactive();
            }

            m_stateStack.AddLast(flowState);

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
                FlowState popState = m_stateStack.Last.Value;
                popState.OnInactive();
                m_stateStack.RemoveLast();;
                
                if (m_stateStack.Count > 0)
                {
                    FlowState topState = m_stateStack.Last.Value;
                    topState.OnActive();
                    topState.m_stage = FlowState.StateStage.ACTIVE;
                }
            }
        }
        
        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        private void RemoveStateFromStack(FlowState state)
        {
            if (m_stateStack.Count > 0)
            {
                bool lastState = m_stateStack.Last.Value == state;
                state.OnInactive();
                m_stateStack.Remove(state);
     
                if (m_stateStack.Count > 0 && lastState)
                {
                    FlowState topState = m_stateStack.Last.Value;
                    topState.OnActive();
                    topState.m_stage = FlowState.StateStage.ACTIVE;
                }
            }
        }

        /// <summary>
        /// Pops all of the states belonging to this state machine
        /// </summary>
        public override void PopAllStates()
        {
            foreach (FlowState flowState in m_stateStack)
            {
                RemoveState(flowState);
            }
        }
    }
}