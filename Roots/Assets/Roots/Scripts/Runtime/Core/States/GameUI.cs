using Siren;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameUI : FlowScreenUI
{
    [SerializeField] private EconomyUI _economyUI;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private Camera _overlayCamera;

    public VegetableStockData _vegetableStockData;
    public EconomyData _economyData;
    
    public override void InitUI()
    {
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(_overlayCamera);
    }

    public override void UpdateUI()
    {
        _inventoryUI.UpdateStockCounts(_vegetableStockData.VegetableCropStock, _vegetableStockData.VegetableSeedStock);
        _economyUI.UpdateBalance(_economyData.Debt, _economyData.Balance);
    }

    public override void DestroyUI()
    {
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Remove(_overlayCamera);
    }
}
