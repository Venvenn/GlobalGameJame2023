using Mono.Cecil;
using Siren;
using Unity.Mathematics;
using UnityEngine;

public class FSGarden : FlowState
{
    private GardenUI _ui;
    private UIManager _uiManager;
    private Camera _camera;

    private AllVegetables _allVegetables;
    private VegetableStockData _vegetableStockData;

    private GridSystem _gridSystem;
    private VegetableSystem _vegetableSystem;
    private CameraController _cameraController;
    private EconomyData _economyData;
    
    //temp data
    private int _selectedType = -1;
    private int2 _hoverCell;
    
    public FSGarden(UIManager uiManager, VegetableStockData vegetableStockData, EconomyData economyData)
    {
        //UI
        _uiManager = uiManager;

        //Camera
        _camera = Camera.main;
        _cameraController = _camera.GetComponentInParent<CameraController>();

        //Systems
        SquareGridComponent gridComponent = Object.Instantiate(Resources.Load<SquareGridComponent>("Prefabs/Grid"));
        gridComponent.transform.position = new Vector3(gridComponent.transform.position.x, 0.16f, gridComponent.transform.position.z);
        _gridSystem = new GridSystem(gridComponent, _camera);
        _vegetableSystem = new VegetableSystem();

        //Data
        _allVegetables = Resources.Load<AllVegetables>("Data/AllVegetables");
        _economyData = economyData;
        _vegetableStockData = vegetableStockData;
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<GardenUI>("UI/Screens/GardenUI", this);
        _cameraController.SnapCamera(new Vector3(_gridSystem.Size.x / 2f, 0, _gridSystem.Size.y / 2f), _cameraController.CameraSettings.MaxZoom);
    }

    public override void OnActive()
    {
        _ui.gameObject.SetActive(true);
    }


    public override void ActiveUpdate()
    {
        //Vegetables
        _vegetableSystem.UpdateVegetables(_gridSystem, _allVegetables);

        //Grid Highlight
        _gridSystem.DeselectCell(_hoverCell);
        _hoverCell = _gridSystem.GetCellPosFromPointer(Input.mousePosition);
        _gridSystem.SelectCell(_hoverCell);

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

        if (Input.GetMouseButtonDown(1))
        {
            PluckVegetable(_hoverCell);
        }

        _ui.UpdateUI();        
    }
    
    private void FocusGridSpace(int2 selectedCell)
    {
        if (_gridSystem.CellValid(selectedCell))
        {
            _cameraController.MoveTo(_gridSystem.GetWorldPosition(selectedCell));
            if (_cameraController.IsZoomOut)
            {
                _cameraController.Zoom(_cameraController.CameraSettings.MinZoom);
            }  
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

    private void TryPlaceOnGrid(VegetableData vegetableData)
    {
        if (_gridSystem.CellValid(_hoverCell) && !_gridSystem.HasEntity(_hoverCell) && vegetableData.Prefab != null && _vegetableStockData.VegetableStock[_selectedType] > 0)
        {
            PlaceOnGrid(vegetableData);
            _vegetableStockData.VegetableStock[_selectedType]--;
        }
    }

    private void PluckVegetable(int2 grid)
    {
        GridData vegData = new GridData();
        if (_gridSystem.GetEntity(grid, out vegData))
        {
            _vegetableStockData.VegetableStock[vegData.TypeId] += (int)(vegData.Data.GetYield() * _allVegetables.VegetableDataObjects[vegData.TypeId].VegetableData.HarvestNumber);
            _gridSystem.RemoveEntityFromGrid(grid);
            //_gridSystem.Unhighlight(grid);
        }
    }

    private void PlaceOnGrid(VegetableData vegetableData)
    {
        VegetableObject gameObject = Object.Instantiate(vegetableData.Prefab);
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.position = _gridSystem.GetWorldPosition(_hoverCell);
        GridData gridData = new GridData()
        {
            TypeId = _selectedType,
            CellId = _hoverCell,
            GridObject = gameObject,
            Data = new VegetableStateData(0, vegetableData.MaxHealth, 1, TimeSystem.GetTimeDate())
        };
        _gridSystem.AddEntityToGrid(gridData, _hoverCell);
        //_gridSystem.HighlightCell(_hoverCell, new Color(0.4f, 0.2f, 0.1f, 0.3f));
    }

    public override void ActiveFixedUpdate()
    {
    }

    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
            case PlaceVegetableFlowMessage placeVegetableFlowMessage:
                _selectedType = placeVegetableFlowMessage.VegetableType;
                TryPlaceOnGrid(_allVegetables.VegetableDataObjects[placeVegetableFlowMessage.VegetableType].VegetableData);
                break;
            case NewMonthMessage newMonthMessage:
                OnNewMonth();
                break;
        }
    }

    private void OnNewMonth()
    {
        if (!_economyData.MonthTick())
        {
            FlowStateMachine.Push(new FSEnd(_uiManager, false));
        }
        else if (_economyData.Debt <= 0)
        {
            FlowStateMachine.Push(new FSEnd(_uiManager, true));
        }
        else
        {
            //Merchant
            FlowStateMachine.Push(new FSShop(_uiManager, _vegetableStockData));   
        }
    }

    public override void OnInactive()
    {
        _ui.gameObject.SetActive(false);
    }

    public override void OnDismiss()
    {
    }

    public override void FinishDismiss()
    {
        Object.Destroy(_ui.gameObject);
        _gridSystem.Destroy();
    }
}
