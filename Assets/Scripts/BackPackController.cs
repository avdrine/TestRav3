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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryEquipItem(PhysicalItem item)
    {
        if(item.ItemData != null)
        {
            switch (item.ItemData.Type)
            {
                case ItemType.MeleeWeapon:
                    if (_equippedMeleeWeapon == null)
                    {
                        _equippedMeleeWeapon = item;
                        _equippedMeleeWeapon.MoveToInventory(_pointToEquippedMeleeWeapon);
                    }
                    break;
                case ItemType.SmallItem:
                    if (_equippedSmallItem == null)
                    {
                        _equippedSmallItem = item;
                        _equippedSmallItem.MoveToInventory(_pointToEquippedSmallItem);
                    }
                    break;
                case ItemType.SmallRiffle:
                    if (_equippedSmallRiffle == null)
                    {
                        _equippedSmallRiffle = item;
                        _equippedSmallRiffle.MoveToInventory(_pointToEquippedSmallRiffle);
                    }
                    break;
            }
        }
        
    }

}
