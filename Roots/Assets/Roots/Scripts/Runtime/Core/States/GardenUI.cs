using Siren;
using UnityEngine;

public class GardenUI : FlowScreenUI
{
    public VegetableStockData _vegetableStockData;
    public EconomyData _economyData;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private EconomyUI _economyUI;

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
