using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] InventoryUI _inventoryUI;
    [SerializeField] private Image _iconImage;
    [SerializeField] private int _vegetableID;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private TMPro.TextMeshProUGUI _stockCount;

    public int VegetableID => _vegetableID;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cursor.SetActive(true);
        _inventoryUI.UpdateCursor(VegetableID);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cursor.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cursor.SetActive(false);
        _inventoryUI.SendVegetableMessage(_vegetableID);
    }

    public void SetStock(int amount)
    {
        _stockCount.text = amount.ToString();
    }
}
