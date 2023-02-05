using System;

namespace Siren
{
    /// <summary>
    /// State to be used within flow state machine, should contain contiguous game logic  
    /// </summary>
    [Serializable]
    public abstract class FlowState
    {
        public enum StateStage
        {
            PRESENTING,
            ACTIVE,
            INACTIVE,
            DISMISSING,
        }

        public enum TransitionState
        {
            IN_PROGRESS,
            COMPLETED
        }

        /// <summary>
        /// the flow state machine that ownes this state
        /// </summary>
        public BaseFlowStateMachine FlowStateMachine = null;

        /// <summary>
        /// the current stage this state is on
        /// </summary>
        public StateStage m_stage = StateStage.INACTIVE;

        /// <summary>
        /// Called when the state is first added to the stack
        /// </summary>
        public virtual void OnInitialise()
        {
        }

        /// <summary>
        /// called every tick until TransitionState.COMPLETED is returned, things like loading and transitions can be done here 
        /// </summary>
        /// <returns></returns>
        public virtual TransitionState UpdateInitialise()
        {
            return TransitionState.COMPLETED;
        }

        /// <summary>
        /// called when the initialising has been completed
        /// </summary>
        public virtual void FinishInitialise()
        {
        }

        /// <summary>
        /// called either when the full initialising process has been completed or this state becomes active again after being inactive 
        /// </summary>
        public virtual void OnActive()
        {
        }

        /// <summary>
        /// called when another state is pushed onto the stack and this state becomes no longer active
        /// </summary>
        public virtual void OnInactive()
        {
        }

        /// <summary>
        /// called every tick when this state is active
        /// </summary>
        public virtual void ActiveUpdate()
        {
        }

        /// <summary>
        /// called at unity's fixed time step which by default is 0.02 seconds, should be used for physics 
        /// </summary>
        public virtual void ActiveFixedUpdate()
        {
        }
        
        /// <summary>
        /// called drawing Shapes
        /// </summary>
        public virtual void OnRender()
        {
        }

        /// <summary>
        /// called when this state is popped 
        /// </summary>
        public virtual void OnDismiss()
        {
        }

        /// <summary>
        /// called every tick while in the dismissing stage until TransitionState.COMPLETED is returned 
        /// </summary>
        /// <returns></returns>
        public virtual TransitionState UpdateDismiss()
        {
            return TransitionState.COMPLETED;
        }

        /// <summary>
        /// called when the dismissing of the state has been completed
        /// </summary>
        public virtual void FinishDismiss()
        {
        }

        /// <summary>
        /// any flow message sent while this state is active will be revived here 
        /// </summary>
        public virtual void ReceiveFlowMessages(object message)
        {
        }

        /// <summary>
        /// Sends a flow message through the state machine
        /// </summary>
        public void SendFlowMessage(object message, FlowState state)
        {
            FlowStateMachine.SendFlowMessage(message, state);
        }
    }
}