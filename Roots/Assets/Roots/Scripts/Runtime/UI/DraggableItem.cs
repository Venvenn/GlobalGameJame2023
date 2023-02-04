using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] InventoryUI _inventoryUI;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _cursorImage;
    [SerializeField] private int _vegetableID;
    [SerializeField] private TMPro.TextMeshProUGUI _stockCount;

    public int VegetableID => _vegetableID;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cursorImage.sprite = _iconImage.sprite;
        _cursorImage.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cursorImage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cursorImage.gameObject.SetActive(false);
        _inventoryUI.SendVegetableMessage(_vegetableID);
    }

    public void SetStock(int amount)
    {
        _stockCount.text = amount.ToString();
    }
}
