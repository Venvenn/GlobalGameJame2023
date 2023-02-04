using Siren;
using Unity.Mathematics;
using UnityEngine;

public class FSGame : FlowState
{
    private GameUI _ui;
    private UIManager _uiManager;
    private FlowStateMachine _gameplayStates;
    private GridSystem _gridSystem;
    private Camera _camera;

    public FSGame(UIManager uiManager)
    {
        //UI
        _uiManager = uiManager;
        _gameplayStates = new FlowStateMachine(this);
        
        //Camera
        _camera = Camera.main;
        
        //Grid
        SquareGridComponent gridComponent = Object.Instantiate(Resources.Load<SquareGridComponent>("Prefabs/Grid"));
        _gridSystem = new GridSystem(gridComponent, _camera);
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<GameUI>("UI/Screens/GameUI", this);
    }

    public override void OnActive()
    {
    }

    public override void ActiveUpdate()
    {
        _gameplayStates.Update();
        
        //temp input 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _gridSystem.ClearAllSelectedCells();
            int2 cellPosFromPointer = _gridSystem.GetCellPosFromPointer(Input.mousePosition);
            Debug.Log(cellPosFromPointer);
            _gridSystem.SelectCell(cellPosFromPointer);
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
        _gridSystem.Destroy();
    }
}
