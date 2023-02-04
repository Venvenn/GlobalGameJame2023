using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _cursorImage;

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
    }
}
