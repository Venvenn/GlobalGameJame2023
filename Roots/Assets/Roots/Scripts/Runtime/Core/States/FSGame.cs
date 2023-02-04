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
    private CameraController _cameraController;

    //temp data
    private int _selectedType = 0;
    private int2 _hoverCell;
    
    public FSGame(UIManager uiManager)
    {
        //UI
        _uiManager = uiManager;
        _gameplayStates = new FlowStateMachine(this);
        
        //Camera
        _camera = Camera.main;
        _cameraController = _camera.GetComponentInParent<CameraController>();
        
        //Grid
        SquareGridComponent gridComponent = Object.Instantiate(Resources.Load<SquareGridComponent>("Prefabs/Grid"));
        _gridSystem = new GridSystem(gridComponent, _camera);
        
        //Time
        Time.timeScale = 1;
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<GameUI>("UI/Screens/GameUI", this);
        _cameraController.SnapCamera(new Vector3(_gridSystem.Size.x / 2f, 0,  _gridSystem.Size.y / 2f), _cameraController.CameraSettings.MaxZoom);
    }

    public override void OnActive()
    {
    }

    public override void ActiveUpdate()
    {
        _gameplayStates.Update();
        
        //Grid Highlight
        _gridSystem.DeselectCell(_hoverCell);
        _hoverCell = _gridSystem.GetCellPosFromPointer(Input.mousePosition);
        _gridSystem.SelectCell(_hoverCell);
        
        //temp input 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedType = _selectedType == 0 ? 1 : 0;
        }
        
        if (Input.mouseScrollDelta.y > 0 && _cameraController.IsZoomOut)
        {
            FocusGridSpace(_hoverCell);
        }
        else if (Input.mouseScrollDelta.y < 0 && !_cameraController.IsZoomOut)
        {
            ZoomOut();
        }

        //if you click a plot while zoomed in, focus it
        if (Input.GetMouseButtonUp(0) && !_cameraController.IsZoomOut)
        {
            FocusGridSpace(_hoverCell);
        }
        
        if (Input.GetMouseButtonDown(0) && _selectedType != 0)
        {
            PlaceOnGrid();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            _gridSystem.RemoveEntityFromGrid(_hoverCell);
        }
    }

    private void FocusGridSpace(int2 selectedCell)
    {
        _cameraController.MoveTo(_gridSystem.GetWorldPosition(selectedCell));
        if (_cameraController.IsZoomOut)
        {
            _cameraController.Zoom(_cameraController.CameraSettings.MinZoom);
        }
    }

    private void ZoomOut()
    {
        _cameraController.MoveTo(new Vector3(_gridSystem.Size.x / 2f, 0, _gridSystem.Size.y / 2f));
        if (!_cameraController.IsZoomOut)
        {
            _cameraController.Zoom(_cameraController.CameraSettings.MaxZoom);
        }
    }

    private void PlaceOnGrid()
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.transform.position = _gridSystem.GetWorldPosition(_hoverCell);
        gameObject.transform.localScale *= 0.5f;
        GridData gridData = new GridData()
        {
            TypeId = _selectedType,
            GridObject = gameObject
        };
        _gridSystem.AddEntityToGrid(gridData,_hoverCell);
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
