using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Контроллер ячейки инвентаря
/// </summary>
public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Переменные

    /// <summary>
    /// Предмет в ячейке (nullable)
    /// </summary>
    [SerializeField] private PhysicalItem _item;

    /// <summary>
    /// Image для иконки предмета
    /// </summary>
    [SerializeField] private Image _image;

    /// <summary>
    /// Событие выкидывания предмета из ячейки
    /// </summary>
    [SerializeField] private PhysicalItemEvent _dropEvent = new PhysicalItemEvent();

    /// <summary>
    /// Событие что указатель мыши начал перемещаться по ячейке
    /// </summary>
    [SerializeField] private UnityEvent _pointerEnter = new UnityEvent();

    /// <summary>
    /// Событие что указатель мыши вышел за пределы ячейки
    /// </summary>
    [SerializeField] private UnityEvent _pointerExit = new UnityEvent();

    /// <summary>
    /// Флаг нахождения указателя мыши внутри ячейки
    /// </summary>
    private bool _mouseEnterCell = false;

    #endregion

    #region Свойства


    /// <summary>
    /// Предмет в ячейке (nullable)
    /// </summary>
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

    /// <summary>
    /// Событие выкидывания предмета из ячейки
    /// </summary>
    public PhysicalItemEvent DropEvent { get => _dropEvent; }

    /// <summary>
    /// Событие что указатель мыши начал перемещаться по ячейке
    /// </summary>
    public UnityEvent PointerEnter { get => _pointerEnter; }

    /// <summary>
    /// Событие что указатель мыши вышел за пределы ячейки
    /// </summary>
    public UnityEvent PointerExit { get => _pointerExit;  }

    #endregion

    #region Функции

    public void Update()
    {
        //Если мышка отпущена внутри ячейки и в ячейке есть предмет - выкинуть его
        if(_mouseEnterCell && Input.GetKeyUp(KeyCode.Mouse0) && Item != null)
        {
            DropItem();
        }
    }

    /// <summary>
    /// Выкинуть предмет из ячейки
    /// </summary>
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
        if(Item !=null)
        {
            _mouseEnterCell = true;
            PointerEnter.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item != null)
        {
            _mouseEnterCell = false;
            PointerExit.Invoke();
        }
    }

    #endregion
}
