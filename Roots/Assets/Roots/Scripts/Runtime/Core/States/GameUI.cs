using Siren;
using UnityEngine;

public class GameUI : FlowScreenUI
{
    public VegetableStockData _vegetableStockData;
    [SerializeField] private InventoryUI _inventoryUI;
    
    public override void InitUI()
    {

    }

    public override void UpdateUI()
    {
        _inventoryUI.UpdateStockCounts(_vegetableStockData.VegetableStock);
    }

    public override void DestroyUI()
    {

    }
}
