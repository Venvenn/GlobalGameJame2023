using Siren;
using UnityEngine;

public class FSGame : FlowState
{
    private GameUI m_ui;
    private UIManager m_uiManager;
    private FlowStateMachine m_gameplayStates;

    public FSGame(UIManager uiManager)
    {
        //UI
        m_uiManager = uiManager;
        m_gameplayStates = new FlowStateMachine(this);
    }

    public override void OnInitialise()
    {
        m_ui = m_uiManager.LoadUIScreen<GameUI>("UI/Screens/GameUI", this);
    }

    public override void OnActive()
    {
    }

    public override void ActiveUpdate()
    {
        m_gameplayStates.Update();
        
        //temp input 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    
    public override void ActiveFixedUpdate()
    {
        m_gameplayStates.FixedUpdate();
    }

    private void Pause()
    {
        FlowStateMachine.Push(new FSPauseMenu(this, m_uiManager));
    }
    
    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case "mainMenu":
            {
                FlowStateMachine.Pop();
                break;
            }
            case "pauseMenu":
            {
                Pause();
                break;
            }
        }
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
        m_gameplayStates.PopAllStates();
    }

    public override TransitionState UpdateDismiss()
    {
        m_gameplayStates.Update();
        if(m_gameplayStates.StateCount == 0)
        {
            return TransitionState.COMPLETED;
        }
        return TransitionState.IN_PROGRESS;
    }

    public override void FinishDismiss()
    {
        Object.Destroy(m_ui.gameObject);
    }
}
