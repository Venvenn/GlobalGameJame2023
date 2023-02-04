using Siren;
using UnityEngine;

public class FSSystem : FlowState
{
    private UIManager m_uiManager;
    private FlowStateMachine m_gameStates;

    public FSSystem()
    {
        //UI
        m_uiManager = new UIManager("UI/UIScreens");
        m_gameStates = new FlowStateMachine(this);
        
        TimeSystem.Init(Resources.Load<TimeSettings>("Data/TimeData"));
        Time.timeScale = 0;
    }

    public override void OnInitialise()
    {
        m_gameStates.Push(new FSMainMenu(m_uiManager));
    }

    public override void OnActive()
    {
    }

    public override void ActiveUpdate()
    {
        m_gameStates.Update();
        TimeSystem.UpdateTime();
    }
    
    public override void ActiveFixedUpdate()
    {
        m_gameStates.FixedUpdate();
    }
    
    public override void ReceiveFlowMessages(object message)
    {
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
        m_gameStates.PopAllStates();
    }

    public override TransitionState UpdateDismiss()
    {
        m_gameStates.Update();
        if(m_gameStates.StateCount == 0)
        {
            return TransitionState.COMPLETED;
        }
        return TransitionState.IN_PROGRESS;
    }

    public override void FinishDismiss()
    {
    }
}
