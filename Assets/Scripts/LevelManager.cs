using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private BackPackController _playerBackPack;
    [SerializeField] private bool _dragItem = false;

    public bool IsDragItem { get => _dragItem; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnItemDropToBackPack(PhysicalItem item)
    {
        _playerBackPack.TryEquipItem(item);
    }

    public void OnItemThrowFromBackPack()
    {

    }
    
    public void OnStartDragItem()
    {
        _dragItem = true;
    }

    public void OnEndDragItem()
    {
        _dragItem = false;
    }
}
