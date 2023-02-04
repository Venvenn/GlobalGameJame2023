using Mono.Cecil;
using Siren;
using Unity.Mathematics;
using UnityEngine;

public class FSGarden : FlowState
{
    private GardenUI _ui;
    private UIManager _uiManager;
    private GridSystem _gridSystem;
    private Camera _camera;

    private AllVegetables _allVegetables;
    
    //temp data
    private int _selectedType = -1;
    private int2 _hoverCell;
    
    public FSGarden(UIManager uiManager)
    {
        //UI
        _uiManager = uiManager;

        //Camera
        _camera = Camera.main;
        
        //Grid
        SquareGridComponent gridComponent = Object.Instantiate(Resources.Load<SquareGridComponent>("Prefabs/Grid"));
        _gridSystem = new GridSystem(gridComponent, _camera);
        
        //Data
        _allVegetables = Resources.Load<AllVegetables>("Data/AllVegetables");
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<GardenUI>("UI/Screens/GardenUI", this);
    }

    public override void OnActive()
    {
    }

    public override void ActiveUpdate()
    {
        //Grid Highlight
        _gridSystem.DeselectCell(_hoverCell);
        _hoverCell = _gridSystem.GetCellPosFromPointer(Input.mousePosition);
        _gridSystem.SelectCell(_hoverCell);

        //Temp Input
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _selectedType = -1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedType = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedType = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _selectedType = 2;
        }
        
        if (Input.GetMouseButtonDown(0) && _selectedType != -1)
        {
            PlaceOnGrid(_allVegetables.VegetableDataObjects[_selectedType].VegetableData);
        }
        if (Input.GetMouseButtonDown(1))
        {
            _gridSystem.RemoveEntityFromGrid(_hoverCell);
        }
    }

    private void PlaceOnGrid(VegetableData vegetableData)
    {
        VegetableObject gameObject = Object.Instantiate(vegetableData.Prefab);
        gameObject.transform.position = _gridSystem.GetWorldPosition(_hoverCell);
        GridData gridData = new GridData()
        {
            TypeId = _selectedType,
            GridObject = gameObject,
            Data = new VegetableStateData(0, vegetableData.MaxHealth, 1)
        };
        _gridSystem.AddEntityToGrid(gridData,_hoverCell);
    }
    
    public override void ActiveFixedUpdate()
    {
    }

    public override void ReceiveFlowMessages(object message)
    {
        switch (message)
        {
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
        _gridSystem.Destroy();
    }
}
