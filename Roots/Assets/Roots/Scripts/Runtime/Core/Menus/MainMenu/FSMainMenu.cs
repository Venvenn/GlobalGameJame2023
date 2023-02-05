using Siren;
using UnityEngine;

public class FSMainMenu : FlowState
{
    private MainMenuUI m_ui;
    private UIManager m_uiManager;
    public FSMainMenu(UIManager uiManager)
    {
        m_uiManager = uiManager;
    }

    public override void OnInitialise()
    {
        //Set up scene
        m_ui = m_uiManager.LoadUIScreen<MainMenuUI>("UI/Screens/MainMenuUI", this);
   
        //Set up UI
        m_ui.GetComponent<FlowUIGroup>().AttachFlowState(this);
    }

    public override void OnActive()
    {
        m_ui.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        FlowStateMachine.Push(new FSGame(m_uiManager));
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
    
    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case "start":
            {
                StartGame();
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

    public override void OnDismiss()
    {
        Object.Destroy(m_ui.gameObject);
    }
}
