using Siren;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlaceVegetableFlowMessage _placeVegetableFlowMessage;
    [SerializeField] private FlowUIGroup _flowUIGroup;

    public void SendVegetableMessage(int vegetableId)
    {
        _placeVegetableFlowMessage.VegetableType = vegetableId;
        _flowUIGroup.SendMessage(_placeVegetableFlowMessage);
    }
}
