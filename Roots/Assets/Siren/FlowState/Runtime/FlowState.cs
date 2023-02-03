namespace Siren
{
    /// <summary>
    /// A flow state is a base type that controls the game flow, all game flow states should inherit from this
    /// </summary>
    public abstract class FlowState
    {
        public enum StateStage
        {
            PRESENTING,
            ACTIVE,
            INACTIVE,
            DISMISSING
        }

        public enum TransitionState
        {
            IN_PROGRESS,
            COMPLETED
        }

        public FlowStateMachine FlowStateMachine = null;
        public StateStage Stage = StateStage.INACTIVE;

        public virtual void StartPresenting()
        {
        }

        public virtual TransitionState UpdatePresenting()
        {
            return TransitionState.COMPLETED;
        }

        public virtual void FinishPresenting()
        {
        }

        public virtual void OnActive()
        {
        }

        public virtual void OnInactive()
        {
        }

        public virtual void ActiveUpdate()
        {
        }

        public virtual void ActiveFixedUpdate()
        {
        }

        public virtual void StartDismissing()
        {
        }

        public virtual TransitionState UpdateDismissing()
        {
            return TransitionState.COMPLETED;
        }

        public virtual void FinishDismissing()
        {
        }

        public virtual void ReceiveFlowMessages(object message)
        {
        }
    }
}