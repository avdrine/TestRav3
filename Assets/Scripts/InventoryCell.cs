using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private ItemData _item;
    [SerializeField] private Image _image;
    [SerializeField] private ItemDataEvent _dropEvent = new ItemDataEvent();

    public ItemData Item 
    { 
        get => _item;
        set
        {
            _item = value;
            if (_item != null) { _image.sprite = _item.Icon; _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f); }
            else { _image.sprite = null; _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f); }
        }
    }

    public ItemDataEvent DropEvent { get => _dropEvent; }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(Item != null)
        {
            DropEvent.Invoke(Item);
        }
    }

}
