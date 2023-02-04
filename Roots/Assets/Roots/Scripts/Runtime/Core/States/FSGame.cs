using Siren;
using Unity.Mathematics;
using UnityEngine;

public class FSGame : FlowState
{
    private GameUI _ui;
    private UIManager _uiManager;
    private FlowStateMachine _gameplayStates;
    
    private AllVegetables _allVegetables;
    private VegetableStockData _vegetableStockData;
    private EconomyData _economyData;
    
    public FSGame(UIManager uiManager)
    {
        //UI
        _uiManager = uiManager;
        _gameplayStates = new FlowStateMachine(this);
        
        //Time
        Time.timeScale = 1;
        
        //Data
        _allVegetables = Resources.Load<AllVegetables>("Data/AllVegetables");
        _economyData = new EconomyData(Resources.Load<EconomyDataSettings>("Data/EconomySettings"));
        _vegetableStockData = new VegetableStockData(_allVegetables);
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<GameUI>("UI/Screens/GameUI", this);
        _ui._vegetableStockData = _vegetableStockData;
        _ui._economyData = _economyData;
        _gameplayStates.Push(new FSGarden(_uiManager, _vegetableStockData, _economyData));
    }

    public override void OnActive()
    {
        
    }

    public override void ActiveUpdate()
    {
        _gameplayStates.Update();
        _ui.UpdateUI();
        //temp input 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
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
            case PlaceVegetableFlowMessage vegetablePlaceMessage:
            {
                _gameplayStates.SendFlowMessage(vegetablePlaceMessage, _gameplayStates.GetTopState());
                break;
            }
            case NewMonthMessage newMonthMessage:
            {
                _gameplayStates.SendFlowMessage(newMonthMessage, _gameplayStates.GetTopState());
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
