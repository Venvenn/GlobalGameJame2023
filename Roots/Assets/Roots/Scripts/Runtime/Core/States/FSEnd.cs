using Siren;
using Unity.Mathematics;
using UnityEngine;

public class FSEnd : FlowState
{
    private EndUI _ui;
    private UIManager _uiManager;
    private bool _win;
    
    public FSEnd(UIManager uiManager, bool win)
    {
        _win = win;
        
        //UI
        _uiManager = uiManager;

        //Time
        Time.timeScale = 0;
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<EndUI>("UI/Screens/EndUI", this);
        _ui.SetUp(_win);
    }

    public override void OnActive()
    {
        
    }

    public override void ActiveUpdate()
    {
        _ui.UpdateUI();
    }
    
    public override void ActiveFixedUpdate()
    {
    }
    
    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case "mainMenu":
            {
                FlowStateMachine.m_owningState.SendFlowMessage(message, FlowStateMachine.m_owningState);
                break;
            }
        }
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
    }

    public override void FinishDismiss()
    {
        Object.Destroy(_ui.gameObject);
    }
}
