using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер рюкзака
/// </summary>
public class BackPackController : MonoBehaviour
{
    #region Переменные

    /// <summary>
    /// Одетый предмет класса "маленькое ружье" (nullable)
    /// </summary>
    private PhysicalItem _equippedSmallRiffle;
    /// <summary>
    /// Одетый предмет класса "оружие ближнего боя" (nullable)
    /// </summary>
    private PhysicalItem _equippedMeleeWeapon;
    /// <summary>
    /// Одетый предмет класса "маленький предмет" (nullable)
    /// </summary>
    private PhysicalItem _equippedSmallItem;

    /// <summary>
    /// Место для одетого объекта на рюкзаке класса "маленькое ружье"
    /// </summary>
    [SerializeField] private Transform _pointToEquippedSmallRiffle;

    /// <summary>
    /// Место для одетого объекта на рюкзаке класса "оружие ближнего боя"
    /// </summary>
    [SerializeField] private Transform _pointToEquippedMeleeWeapon;

    /// <summary>
    /// Место для одетого объекта на рюкзаке класса "маленький предмет"
    /// </summary>
    [SerializeField] private Transform _pointToEquippedSmallItem;

    /// <summary>
    /// UI для инвентаря
    /// </summary>
    [SerializeField] private GameObject _backPackUI;

    /// <summary>
    /// Ячейки инвентаря
    /// </summary>
    [SerializeField] private List<InventoryCell> _inventoryCells = new List<InventoryCell>();

    /// <summary>
    /// Флаг нахождения мышки на ячейке инвентаря
    /// </summary>
    private bool _mouseOnCell = false;

    #endregion

    #region Свойства

    /// <summary>
    /// Флаг отсутствия свободного места в инвентаре
    /// </summary>
    private bool IsInventoryFull { 
        get 
        {
            for (int i = 0; i < _inventoryCells.Count; i++)
            {
                if (_inventoryCells[i].Item == null) return false;
            }
            return true;
        } 
    }

    #endregion

    #region Функции

    void Start()
    {
        InitilizeInventoryGUI();
    }

    void Update()
    {
        //Если интерфейс рюкзака активен, была отпущена мышь и при этом мышь не наведена на ячейку инвентаря - закрыть UI
        if (_backPackUI.activeSelf && Input.GetKeyUp(KeyCode.Mouse0) && !_mouseOnCell)
            CloseBackPackUI();
    }

    public void MouseEnterCell()
    {
        _mouseOnCell = true;
    }
    public void MouseExitCell()
    {
        _mouseOnCell = false;
    }

    /// <summary>
    /// Закрыть UI рюкзака
    /// </summary>
    public void CloseBackPackUI()
    {
        _backPackUI.SetActive(false);
        _mouseOnCell = false;
    }

    /// <summary>
    /// Инициализация UI рюкзака
    /// </summary>
    private void InitilizeInventoryGUI()
    {
        TryAddItemToCells(_equippedSmallRiffle);
        TryAddItemToCells(_equippedMeleeWeapon);
        TryAddItemToCells(_equippedSmallItem);
        for (int i = 0; i < _inventoryCells.Count; i++)
        {
            if (_inventoryCells[i].Item == null) _inventoryCells[i].Item = null;

            _inventoryCells[i].DropEvent.AddListener(DropItem);
            _inventoryCells[i].PointerEnter.AddListener(MouseEnterCell);
            _inventoryCells[i].PointerExit.AddListener(MouseExitCell);
        }

        _backPackUI.SetActive(false);
    }

    /// <summary>
    /// Попробовать поместить предмет в инвентарь (Проверяет свободные ячейки в инвентаре)
    /// </summary>
    /// <param name="item">Перемещаемый предмет</param>
    private void TryAddItemToCells(PhysicalItem item)
    {
        if (item != null && item.ItemData != null && !IsInventoryFull)
        {
            for (int i = 0; i < _inventoryCells.Count; i++)
            {
                if (_inventoryCells[i].Item == null)
                {
                    _inventoryCells[i].Item = item; 
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Выкинуть предмет из инвентаря
    /// </summary>
    /// <param name="item">Выкидываемый предмет</param>
    private void DropItem(PhysicalItem item)
    {
        bool operationSuccess = false;
        if (_equippedSmallRiffle == item) { _equippedSmallRiffle = null; item.Unequip(); operationSuccess = true;  }
        else if (_equippedMeleeWeapon == item) { _equippedMeleeWeapon = null; item.Unequip(); operationSuccess = true; }
        else if (_equippedSmallItem == item) { _equippedSmallItem = null; item.Unequip(); operationSuccess = true; }
        CloseBackPackUI();

        if (operationSuccess) LevelManager.Instance.UnequipEvent.Invoke(item);
    }


    /// <summary>
    /// Попробовать одеть предмет на рюкзак
    /// </summary>
    /// <param name="item"></param>
    public void TryEquipItem(PhysicalItem item)
    {
        bool operationSuccess = false;
        if(item.ItemData != null && !IsInventoryFull)
        {
            switch (item.ItemData.Type)
            {
                case ItemType.MeleeWeapon:
                    if (_equippedMeleeWeapon == null)
                    {
                        operationSuccess = true;

                        _equippedMeleeWeapon = item;
                        TryAddItemToCells(item);
                        _equippedMeleeWeapon.MoveToInventory(_pointToEquippedMeleeWeapon);
                    }
                    break;
                case ItemType.SmallItem:
                    if (_equippedSmallItem == null)
                    {
                        operationSuccess = true;

                        _equippedSmallItem = item;
                        TryAddItemToCells(item);
                        _equippedSmallItem.MoveToInventory(_pointToEquippedSmallItem);
                    }
                    break;
                case ItemType.SmallRiffle:
                    if (_equippedSmallRiffle == null)
                    {
                        operationSuccess = true;

                        _equippedSmallRiffle = item;
                        TryAddItemToCells(item);
                        _equippedSmallRiffle.MoveToInventory(_pointToEquippedSmallRiffle);
                    }
                    break;
            }
        }

        if (operationSuccess) LevelManager.Instance.EquipEvent.Invoke(item);
    }

    private void OnMouseEnter()
    {
        if(Input.GetKey(KeyCode.Mouse0) && !LevelManager.Instance.IsDragItem)
        {
            _backPackUI.SetActive(true);
        }
    }
    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !LevelManager.Instance.IsDragItem)
        {
            _backPackUI.SetActive(true);
        }
    }

    #endregion
}
