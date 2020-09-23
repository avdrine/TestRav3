using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPackController : MonoBehaviour
{
    private PhysicalItem _equippedSmallRiffle;
    private PhysicalItem _equippedMeleeWeapon;
    private PhysicalItem _equippedSmallItem;
    
    [SerializeField] private Transform _pointToEquippedSmallRiffle;
    [SerializeField] private Transform _pointToEquippedMeleeWeapon;
    [SerializeField] private Transform _pointToEquippedSmallItem;

    [SerializeField] private GameObject _backPackUI;
    [SerializeField] private List<InventoryCell> _inventoryCells = new List<InventoryCell>();

    private bool _mouseOnCell = false;

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

    // Start is called before the first frame update
    void Start()
    {
        //LoadSaves();
        InitilizeInventoryGUI();
    }

    // Update is called once per frame
    void Update()
    {
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
    public void CloseBackPackUI()
    {
        _backPackUI.SetActive(false);
        _mouseOnCell = false;
    }

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

    private void DropItem(PhysicalItem item)
    {
        if (_equippedSmallRiffle == item) { _equippedSmallRiffle = null; item.Unequip();  }
        else if (_equippedMeleeWeapon == item) { _equippedMeleeWeapon = null; item.Unequip();  }
        else if (_equippedSmallItem == item) { _equippedSmallItem = null; item.Unequip();  }
        CloseBackPackUI();
    }



    public void TryEquipItem(PhysicalItem item)
    {
        if(item.ItemData != null && !IsInventoryFull)
        {
            switch (item.ItemData.Type)
            {
                case ItemType.MeleeWeapon:
                    if (_equippedMeleeWeapon == null)
                    {
                        _equippedMeleeWeapon = item;
                        TryAddItemToCells(item);
                        _equippedMeleeWeapon.MoveToInventory(_pointToEquippedMeleeWeapon);
                    }
                    break;
                case ItemType.SmallItem:
                    if (_equippedSmallItem == null)
                    {
                        _equippedSmallItem = item;
                        TryAddItemToCells(item);
                        _equippedSmallItem.MoveToInventory(_pointToEquippedSmallItem);
                    }
                    break;
                case ItemType.SmallRiffle:
                    if (_equippedSmallRiffle == null)
                    {
                        _equippedSmallRiffle = item;
                        TryAddItemToCells(item);
                        _equippedSmallRiffle.MoveToInventory(_pointToEquippedSmallRiffle);
                    }
                    break;
            }
        }
        
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

}
