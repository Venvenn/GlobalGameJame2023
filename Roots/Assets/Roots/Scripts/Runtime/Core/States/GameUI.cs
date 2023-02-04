using Siren;
using UnityEngine;

public class GameUI : FlowScreenUI
{
    [SerializeField] private EconomyUI _economyUI;
    [SerializeField] private InventoryUI _inventoryUI;

    public VegetableStockData _vegetableStockData;
    public EconomyData _economyData;
    
    public override void InitUI()
    {

    }

    public override void UpdateUI()
    {
        _inventoryUI.UpdateStockCounts(_vegetableStockData.VegetableStock);
        _economyUI.UpdateBalance(_economyData.Debt, _economyData.Balance);
    }

    public override void DestroyUI()
    {

    }
}
