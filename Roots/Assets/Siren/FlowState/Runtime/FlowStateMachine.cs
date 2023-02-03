using System.Collections.Generic;

namespace Siren
{
    /// <summary>
    /// The flow state machine is the heart of the Flowstate system that handles the pushing and popping of active states
    /// </summary>
    public class FlowStateMachine
    {
        private readonly Queue<object> m_flowMessages = new Queue<object>();
        private readonly Queue<(StackAction, FlowState)> m_stateActionQueue = new Queue<(StackAction, FlowState)>();

        public FlowState OwningState = null;
        protected Stack<FlowState> m_stateStack = new Stack<FlowState>();

        public void Update()
        {
            if (m_stateStack.Count > 0)
            {
                var flowState = m_stateStack.Peek();
                switch (flowState.Stage)
                {
                    case FlowState.StateStage.PRESENTING:
                    {
                        var transitionState = flowState.UpdatePresenting();
                        if (transitionState == FlowState.TransitionState.COMPLETED)
                        {
                            flowState.FinishPresenting();
                            flowState.OnActive();
                            flowState.Stage = FlowState.StateStage.ACTIVE;
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
                        var transitionState = flowState.UpdateDismissing();
                        if (transitionState == FlowState.TransitionState.COMPLETED)
                        {
                            flowState.FinishDismissing();
                            flowState.Stage = FlowState.StateStage.INACTIVE;
                            PopState();
                            m_stateStack.Peek().Stage = FlowState.StateStage.ACTIVE;
                        }

                        break;
                    }
                }
            }

            UpdateStateStack();
        }

        public void FixedUpdate()
        {
            if (m_stateStack.Count > 0 && m_stateStack.Peek().Stage == FlowState.StateStage.ACTIVE)
                m_stateStack.Peek().ActiveFixedUpdate();
        }

        public void Pop()
        {
            m_stateActionQueue.Enqueue((StackAction.POP, null));
        }

        public void Push(FlowState flowState)
        {
            m_stateActionQueue.Enqueue((StackAction.PUSH, flowState));
        }

        public void SendFlowMessage(object message)
        {
            m_flowMessages.Enqueue(message);
        }

        public FlowState GetTopState()
        {
            if (m_stateStack.Count > 0) return m_stateStack.Peek();

            return null;
        }

        private void ReceiveFlowMessages()
        {
            for (var i = 0; i < m_flowMessages.Count; i++)
            {
                var message = m_flowMessages.Dequeue();
                m_stateStack.Peek().ReceiveFlowMessages(message);
            }
        }

        private void PushState(FlowState flowState)
        {
            if (m_stateStack.Count > 0)
            {
                m_stateStack.Peek().Stage = FlowState.StateStage.INACTIVE;
                m_stateStack.Peek().OnInactive();
            }

            m_stateStack.Push(flowState);

            flowState.FlowStateMachine = this;
            flowState.Stage = FlowState.StateStage.PRESENTING;
            flowState.StartPresenting();
        }

        private void UpdateStateStack()
        {
            if (m_stateActionQueue.Count > 0)
            {
                var action = m_stateActionQueue.Dequeue();
                switch (action.Item1)
                {
                    case StackAction.POP:
                        m_stateStack.Peek().StartDismissing();
                        m_stateStack.Peek().Stage = FlowState.StateStage.DISMISSING;
                        break;
                    case StackAction.PUSH:
                        PushState(action.Item2);
                        break;
                }
            }
        }

        private void PopState()
        {
            if (m_stateStack.Count > 1)
            {
                m_stateStack.Peek().OnInactive();
                m_stateStack.Pop();
                m_stateStack.Peek().OnActive();
            }
        }

        private enum StackAction
        {
            PUSH,
            POP
        }
    }
}