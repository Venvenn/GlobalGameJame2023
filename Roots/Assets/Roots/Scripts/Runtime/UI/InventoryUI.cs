using Siren;
using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlaceVegetableFlowMessage _placeVegetableFlowMessage;
    [SerializeField] private FlowUIGroup _flowUIGroup;
    [SerializeField] private DraggableItem[] _inventoryItems;
    [SerializeField] private GameObject[] _seedCursors;

    public void SendVegetableMessage(int vegetableId)
    {
        _placeVegetableFlowMessage.VegetableType = vegetableId;
        _flowUIGroup.SendMessage(_placeVegetableFlowMessage);
    }

    public void UpdateStockCounts(List<int> cropStocks, List<int> seedStocks)
    {
        for (int i = 0; i < _inventoryItems.Length; i++)
        {
            _inventoryItems[i].SetStock(seedStocks[_inventoryItems[i].VegetableID], cropStocks[_inventoryItems[i].VegetableID]);
        }
    }

    public void UpdateCursor(int vegetableID)
    {
        for (int i = 0; i < _seedCursors.Length; i++)
        {
            _seedCursors[i].gameObject.SetActive(false);
            if (i == vegetableID)
            {
                _seedCursors[i].gameObject.SetActive(true);
            }
        }
    }
}
