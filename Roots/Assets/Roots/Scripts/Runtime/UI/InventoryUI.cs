using Siren;
using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlaceVegetableFlowMessage _placeVegetableFlowMessage;
    [SerializeField] private FlowUIGroup _flowUIGroup;
    [SerializeField] private DraggableItem[] _inventoryItems;

    public void SendVegetableMessage(int vegetableId)
    {
        _placeVegetableFlowMessage.VegetableType = vegetableId;
        _flowUIGroup.SendMessage(_placeVegetableFlowMessage);
    }

    public void UpdateStockCounts(List<int> itemStocks)
    {
        for (int i = 0; i < _inventoryItems.Length; i++)
        {
            _inventoryItems[i].SetStock(itemStocks[_inventoryItems[i].VegetableID]);
        }
    }
}
