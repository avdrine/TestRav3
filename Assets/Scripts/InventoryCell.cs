using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PhysicalItem _item;
    [SerializeField] private Image _image;
    [SerializeField] private PhysicalItemEvent _dropEvent = new PhysicalItemEvent();
    [SerializeField] private UnityEvent _pointerEnter = new UnityEvent();
    [SerializeField] private UnityEvent _pointerExit = new UnityEvent();

    private bool _mouseEnterCell = false;

    public PhysicalItem Item 
    { 
        get => _item;
        set
        {
            _item = value;
            if (_item != null) { _image.sprite = _item.ItemData.Icon; _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f); }
            else { _image.sprite = null; _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f); }
        }
    }

    public PhysicalItemEvent DropEvent { get => _dropEvent; }
    public UnityEvent PointerEnter { get => _pointerEnter; }
    public UnityEvent PointerExit { get => _pointerExit;  }

    public void Update()
    {
        if(_mouseEnterCell && Input.GetKeyUp(KeyCode.Mouse0) && Item != null)
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        if (Item != null)
        {
            DropEvent.Invoke(_item);
            Item = null;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseEnterCell = true;
        PointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseEnterCell = false;
        PointerExit.Invoke();
    }
}
