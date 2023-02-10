using System.Collections.Generic;
using Siren;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FSShop : FlowState
{
    private const int k_randomVegetableNumber = 3;
    
    private ShopUI _ui;
    private UIManager _uiManager;
    private AllVegetables _allVegetables;
    private EconomyData _economyData;

    private List<VegetableDataObject> _vegetableData = new List<VegetableDataObject>();
    private VegetableStockData _stockData;
    
    public FSShop(UIManager uiManager, VegetableStockData stockData, EconomyData economyData)
    {
        //UI
        _uiManager = uiManager;
        Time.timeScale = 0;
        _allVegetables = Resources.Load<AllVegetables>("Data/AllVegetables");
        _stockData = stockData;
        _economyData = economyData;
    }

    public override void OnInitialise()
    {
        _ui = _uiManager.LoadUIScreen<ShopUI>("UI/Screens/ShopUI", this);

        while (_vegetableData.Count < k_randomVegetableNumber)
        {
            VegetableDataObject data = _allVegetables.VegetableDataObjects[Random.Range(0, _allVegetables.VegetableDataObjects.Length)];
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
            case "finish":
            {
                FlowStateMachine.Pop();
                
                if (!_economyData.MonthTick())
                {
                    FlowStateMachine.Push(new FSEnd(_uiManager, false));
                }
                else if (_economyData.Debt <= 0)
                {
                    FlowStateMachine.Push(new FSEnd(_uiManager, true));
                }
                break;
            }
            case "pay":
            {
                int money = 100;
                if (_economyData.Balance >= money)
                {
                    if (_economyData.Charge(money))
                    {
                        _economyData.PayDebt(money);
                        if (_economyData.Debt <= 0)
                        {
                            FlowStateMachine.Push(new FSEnd(_uiManager, true));
                        }
                    }
                }
                break;
            }
            case ShopVegetableFlowMessage shopVegetableFlowMessage:
            {
                if (shopVegetableFlowMessage.Buy)
                {
                    bool success = _economyData.Charge(shopVegetableFlowMessage.ValueChange * shopVegetableFlowMessage.Quantity);
                    if (success)
                    {
                        _stockData.VegetableSeedStock[shopVegetableFlowMessage.VegetableType] += shopVegetableFlowMessage.Quantity;
                    }
                }
                else
                {
                    int quantity = shopVegetableFlowMessage.Quantity;
                    if (shopVegetableFlowMessage.All)
                    {
                        quantity = _stockData.VegetableCropStock[shopVegetableFlowMessage.VegetableType];
                    }
                    if (_stockData.VegetableCropStock[shopVegetableFlowMessage.VegetableType] >= quantity)
                    {
                        _stockData.VegetableCropStock[shopVegetableFlowMessage.VegetableType] -= quantity;
                        _economyData.AddMoney(shopVegetableFlowMessage.ValueChange * quantity);
                    }
                }
                break;
            }

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
