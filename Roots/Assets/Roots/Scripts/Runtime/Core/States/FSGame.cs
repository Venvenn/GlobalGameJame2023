using Siren;
using Unity.Mathematics;
using UnityEngine;

public class FSGame : FlowState
{
    private GameUI _ui;
    private UIManager _uiManager;
    private FlowStateMachine _gameplayStates;
    
    
    public FSGame(UIManager uiManager)
    {
        //UI
        _uiManager = uiManager;
        _gameplayStates = new FlowStateMachine(this);
        
        //Time
        Time.timeScale = 1;
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<GameUI>("UI/Screens/GameUI", this);
        _gameplayStates.Push(new FSGarden(_uiManager));
    }

    public override void OnActive()
    {
        
    }

    public override void ActiveUpdate()
    {
        _gameplayStates.Update();
        RunEvents();
        //temp input 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void RunEvents()
    {
        if (GameEventSystem.DebtCollected())
        {
            //Pay Debt
        }
        if (GameEventSystem.MerchantArrive())
        {
            _gameplayStates.Push(new FSShop(_uiManager));
        }
    }
    
    public override void ActiveFixedUpdate()
    {
        _gameplayStates.FixedUpdate();
    }

    private void Pause()
    {
        FlowStateMachine.Push(new FSPauseMenu(this, _uiManager));
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
            case (PlaceVegetableFlowMessage vegatablePlaceMessage):
            {
                _gameplayStates.SendFlowMessage(vegatablePlaceMessage, _gameplayStates.GetTopState());
                break;
            }
        }
    }

    public override void OnInactive()
    {
    }

    public override void OnDismiss()
    {
        _gameplayStates.PopAllStates();
    }

    public override TransitionState UpdateDismiss()
    {
        _gameplayStates.Update();
        if(_gameplayStates.StateCount == 0)
        {
            return TransitionState.COMPLETED;
        }
        return TransitionState.IN_PROGRESS;
    }

    public override void FinishDismiss()
    {
        Object.Destroy(_ui.gameObject);
    }
}
