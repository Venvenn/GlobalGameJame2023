using System.Collections.Generic;
using Siren;
using UnityEngine;

public class FSShop : FlowState
{
    private const int k_randomVegetableNumber = 3;
    
    private ShopUI _ui;
    private UIManager _uiManager;
    private AllVegetables _allVegetables;

    private List<VegetableData> _vegetableData = new List<VegetableData>();
    
    public FSShop(UIManager uiManager)
    {
        //UI
        _uiManager = uiManager;
        Time.timeScale = 0;
        _allVegetables = Resources.Load<AllVegetables>("Data/AllVegetables");;
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<ShopUI>("UI/Screens/ShopUI", this);

        while (_vegetableData.Count < k_randomVegetableNumber-1)
        {
            VegetableData data = _allVegetables.VegetableDataObjects[Random.Range(0, _allVegetables.VegetableDataObjects.Length)].VegetableData;
            if (!_vegetableData.Contains(data))
            {
                _vegetableData.Add(data);
            }
        }
    }

    public override void OnActive()
    {
        _ui.PopulateShop(_vegetableData);
    }

    public override void ActiveUpdate()
    {

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
        Time.timeScale = 1;
    }

    public override void FinishDismiss()
    {
        Object.Destroy(_ui.gameObject);
    }
}
