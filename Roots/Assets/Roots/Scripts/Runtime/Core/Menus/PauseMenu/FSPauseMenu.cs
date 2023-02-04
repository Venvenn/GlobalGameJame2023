using Siren;
using UnityEngine;

public class FSPauseMenu : FlowState
{
    private PauseMenuUI m_ui;
    private FlowState m_backState;
    private UIManager m_uiManager;
    
    public FSPauseMenu(FlowState backState, UIManager uiManager)
    {
        Time.timeScale = 0;
        m_backState = backState;
        m_uiManager = uiManager;
    }

    public override void OnInitialise()
    {
        //Set up UI
        m_ui = m_uiManager.LoadUIScreen<PauseMenuUI>("UI/Screens/PauseScreenUI", this);
        m_ui.GetComponent<FlowUIGroup>().AttachFlowState(this);
    }

    public override void OnActive()
    {
        m_ui.gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        FlowStateMachine.Pop();
    }
    
    private void BackToMenu()
    {
        FlowStateMachine.Pop();
        FlowStateMachine.SendFlowMessage("mainMenu", m_backState);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
    
    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case "resume":
            {
                ResumeGame();
                break;
            }
            case "mainMenu":
            {
                BackToMenu();
                break;
            }
            case "quit":
            {
                QuitGame();
                break;
            }
        }
    }

    public override void OnInactive()
    {
        m_ui.gameObject.SetActive(false);
    }
    
    public override void FinishDismiss()
    {
        Object.Destroy(m_ui.gameObject);
        Time.timeScale = 1;
    }
}

